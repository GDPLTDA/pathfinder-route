using PathFinder.GeneticAlgorithm.Factories;
using PathFinder.GeneticAlgorithm.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace PathFinder.GeneticAlgorithm
{
    public class GeneticAlgorithmFinder
    {
        List<IGenome> Populations { get; set; } = new List<IGenome>();
        public IFitness Fitness { get; set; }
        public IMutate Mutate { get; set; }
        public ICrossover Crossover { get; set; }
        public ISelection Selection { get; set; }
        public int PopulationSize { get; set; }
        public int GenerationLimit { get; set; }
        public int BestSolutionToPick { get; set; }
        public int Generations { get; set; }

        public GeneticAlgorithmFinder()
        {
            PopulationSize = GASettings.PopulationSize;
            GenerationLimit = GASettings.GenerationLimit;
            BestSolutionToPick = GASettings.BestSolutionToPick;
        }
        public override bool Find(IMap map)
        {
            if (Mutate == null || Crossover == null || Fitness == null || Selection == null)
                throw new System.Exception("GA cant run without all operators");

            var rand = RandomFactory.Rand;
            var startNode = map.StartNode;
            var endNode = map.EndNode;

            for (int i = 0; i < PopulationSize; i++)
                Populations.Add(new Genome(map));

            foreach (var item in Populations)
                item.Fitness = Fitness.Calc(item);

            for (int i = 0; i < GenerationLimit; i++)
            {
                var newpopulations = new List<IGenome>();
                Populations = Populations.OrderBy(o => o.Fitness).ToList();
                for (int j = 0; j < BestSolutionToPick; j++)
                {
                    Populations[j].Fitness = Fitness.Calc(Populations[j]);
                    newpopulations.Add(Populations[j]);
                }

                while (newpopulations.Count < Populations.Count)
                {
                    // Selection
                    var nodemom = Selection.Select(Populations);
                    var nodedad = Selection.Select(Populations);
                    // CrossOver
                    var cross = Crossover.Calc(new CrossoverOperation(nodemom, nodedad));

                    // Mutation
                    nodemom = Mutate.Calc(cross.Mom);
                    nodedad = Mutate.Calc(cross.Dad);

                    // Fitness
                    nodemom.Fitness = Fitness.Calc(nodemom);
                    nodedad.Fitness = Fitness.Calc(nodedad);

                    // Add in new population
                    newpopulations.Add(nodemom);
                    newpopulations.Add(nodedad);
                }
                Populations = newpopulations.ToList();
            }
            Generations = GenerationLimit;

            return false;
        }
        public void Configure(IFitness fItness, IMutate mutate, ICrossover crossover, ISelection selection)
        {
            Mutate = mutate;
            Crossover = crossover;
            Fitness = fItness;
            Selection = selection;
        }
    }
}