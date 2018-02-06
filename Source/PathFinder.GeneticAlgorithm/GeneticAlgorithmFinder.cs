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

        IGenome Best { get; set; }

        int THROTTLE = 1; // quantidade de requests simultaneos

        public GeneticAlgorithmFinder()
        {
            Mutate = MutateFactory.GetImplementation(GASettings.Mutation);
            Crossover = CrossoverFactory.GetImplementation(GASettings.Crossover);
            PopulationSize = GASettings.PopulationSize;
            GenerationLimit = GASettings.GenerationLimit;
            BestSolutionToPick = GASettings.BestSolutionToPick;
            THROTTLE = GASettings.Throttle;
        }
        public async Task<IGenome> FindPathAsync(Roteiro map, IGenome seed = null)
        {
            if (Mutate == null || Crossover == null || Fitness == null || Selection == null)
                throw new System.Exception("GA cant run without all operators");

            var rand = RandomFactory.Rand;
            var startNode = map.Storage;

            Populations.Clear();

            var popusize = PopulationSize;

            if (seed != null)
            {
                Populations.Add(seed);
                popusize--;
            }
            for (int i = 0; i < popusize; i++)
                Populations.Add(Genome.Generator(map));

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
                    await item.CalcRoutesAsync();
            else
                await Populations
                         .ToObservable(NewThreadScheduler.Default)
                         .Select(n => Observable.FromAsync(n.CalcRoutesAsync))
                         .Merge(THROTTLE);
        }
    }
}