using CalcRoute.GeneticAlgorithm.Abstraction;
using CalcRoute.GeneticAlgorithm.Factories;
using CalcRoute.Routes;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace CalcRoute.GeneticAlgorithm
{
    public class GeneticAlgorithmFinder
    {
        List<Genome> Populations { get; set; } = new List<Genome>();
        public IFitness Fitness { get; set; } = FitnessFactory.GetImplementation(FitnessEnum.DistanceAndTime);
        public IMutate Mutate { get; set; }
        public ICrossover Crossover { get; set; }
        public ISelection Selection { get; set; } = SelectionFactory.GetImplementation(SelectionEnum.RouletteWheel);
        public int PopulationSize { get; set; }
        public int GenerationLimit { get; set; }
        public int BestSolutionToPick { get; set; }
        private readonly GASettings Settings;

        private readonly IRouteService routeService;

        Genome Best { get; set; }

        int ProcessChunk = 1; // quantidade de requests simultaneos

        public GeneticAlgorithmFinder(IRouteService routeService, GASettings settings)
        {
            this.routeService = routeService;

            Mutate = MutateFactory.GetImplementation(settings.Mutation, settings);
            Crossover = CrossoverFactory.GetImplementation(settings.Crossover, settings, routeService);
            PopulationSize = settings.PopulationSize;
            GenerationLimit = settings.GenerationLimit;
            BestSolutionToPick = settings.BestSolutionToPick;
            ProcessChunk = settings.Throttle;
            Settings = settings;
        }
        public async Task<Genome> FindPathAsync(Roteiro map, Genome seed = null)
        {
            if (Mutate == null || Crossover == null || Fitness == null || Selection == null)
                throw new System.Exception("GA cant run without all operators");

            var locals = map.Destinations.ToList();
            locals.Add(map.Depot);
            routeService.LoadCache();
            await routeService.Prepare(locals);

            var rand = RandomSingleton.Instance;
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
                var newpopulations = new List<Genome>();

                for (int j = 0; j < BestSolutionToPick; j++)
                    newpopulations.Add(Populations[j]);

                while (newpopulations.Count < Populations.Count)
                {
                    if (newpopulations.Any(e => e.Trucks.SelectMany(l => l.Locals).Count() != map.Destinations.Count))
                        throw new System.Exception();

                    // Selection
                    var (nodemom, nodedad) = Selection.SelectCouple(Populations);

                    // CrossOver
                    var sons = Crossover.Make(nodemom, nodedad);

                    // Mutation
                    sons = sons.Select(s => Mutate.Apply(s)).ToArray();

                    newpopulations.AddRange(sons);

                }
                Populations = newpopulations.ToList();

                await CalcFitness();

                Best = Populations.First();
            }

            routeService.SaveCache();
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
            if (ProcessChunk == 1)
                foreach (var item in Populations)
                    await item.CalcRoutesAsync(routeService);
            else
                await Populations
                         .ToObservable(ThreadPoolScheduler.Instance)
                         .Select(n => Observable.FromAsync(_ => n.CalcRoutesAsync(routeService)))
                         .Merge(ProcessChunk);
        }
    }
}