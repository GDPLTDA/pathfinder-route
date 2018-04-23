using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.GeneticAlgorithm.Factories;
using PathFinder.Routes;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace PathFinder.GeneticAlgorithm
{
    public class GeneticAlgorithmFinder
    {
        List<IGenome> Populations { get; set; } = new List<IGenome>();
        public IFitness Fitness { get; set; } = FitnessFactory.GetImplementation(FitnessEnum.TimePath);
        public IMutate Mutate { get; set; }
        public ICrossover Crossover { get; set; }
        public ISelection Selection { get; set; } = SelectionFactory.GetImplementation(SelectionEnum.RouletteWheel);
        public int PopulationSize { get; set; }
        public int GenerationLimit { get; set; }
        public int BestSolutionToPick { get; set; }
        private readonly GASettings Settings;

        private readonly IRouteService routeService;

        IGenome Best { get; set; }

        int THROTTLE = 1; // quantidade de requests simultaneos

        public GeneticAlgorithmFinder(IRouteService routeService, GASettings settings)
        {
            this.routeService = routeService;

            Mutate = MutateFactory.GetImplementation(settings.Mutation, settings);
            Crossover = CrossoverFactory.GetImplementation(settings.Crossover, settings);
            PopulationSize = settings.PopulationSize;
            GenerationLimit = settings.GenerationLimit;
            BestSolutionToPick = settings.BestSolutionToPick;
            THROTTLE = settings.Throttle;
            Settings = settings;
        }
        public async Task<IGenome> FindPathAsync(Roteiro map, IGenome seed = null)
        {
            if (Mutate == null || Crossover == null || Fitness == null || Selection == null)
                throw new System.Exception("GA cant run without all operators");

            var rand = RandomFactory.Rand;
            var startNode = map.Depot;

            Populations.Clear();

            var popusize = PopulationSize;

            if (seed != null)
            {
                Populations.Add(seed);
                popusize--;
            }

            Populations.AddRange(
                    Enumerable.Range(0, popusize)
                    .Select(_ => Genome.Generator(map, Settings)));

            await CalcFitness();

            for (int i = 0; i < GenerationLimit; i++)
            {
                var newpopulations = new List<IGenome>();

                for (int j = 0; j < BestSolutionToPick; j++)
                    newpopulations.Add(Populations[j]);

                while (newpopulations.Count < Populations.Count)
                {
                    // Selection
                    var (nodemom, nodedad) = Selection.SelectCouple(Populations);

                    // CrossOver
                    var (crossMom, crossDad) = Crossover.Make(nodemom, nodedad);

                    // Mutation
                    nodemom = Mutate.Apply(crossMom);
                    nodedad = Mutate.Apply(crossDad);

                    newpopulations.AddRange(new IGenome[] { nodemom, nodedad });
                }
                Populations = newpopulations.ToList();

                await CalcFitness();

                Best = Populations.First();
            }

            return Best;
        }

        private async Task CalcFitness()
        {
            await CalcGenomeRoutesAsync();
            Populations.ForEach(e => e.CalcFitness(Fitness));
            Populations = Populations.OrderBy(o => o.Fitness).ToList();
        }

        public void Configure(IFitness fItness, IMutate mutate, ICrossover crossover, ISelection selection)
        {
            Mutate = mutate;
            Crossover = crossover;
            Fitness = fItness;
            Selection = selection;
        }

        async Task CalcGenomeRoutesAsync()
        {
            if (THROTTLE == 1)
                foreach (var item in Populations)
                    await item.CalcRoutesAsync(routeService);
            else
                await Populations
                         .ToObservable(NewThreadScheduler.Default)
                         .Select(n => Observable.FromAsync(_ => n.CalcRoutesAsync(routeService)))
                         .Merge(THROTTLE);
        }
    }
}