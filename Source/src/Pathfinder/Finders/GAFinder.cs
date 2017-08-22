using Pathfinder.Abstraction;
using Pathfinder.Factories;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder.Finders
{
    public class GAFinder : AbstractFinder, IGeneticAlgorithm
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
        public GAFinder() : base("Genetic Algorithm")
        {
            SleepUITimeInMs = 200;

            PopulationSize = Constants.POPULATION_SIZE;
            GenerationLimit = Constants.GENERATION_LIMIT;
            BestSolutionToPick = Constants.BEST_SOLUTION_TO_PICK;

        }

        protected void UpdateMaxNodes(IMap map)
        {
            var atualNodes = Populations.Sum(o => o.ListNodes.Count);
            if (atualNodes >= map.GetMaxExpandedNodes())
                map.SetMaxExpandedNodes(atualNodes);
        }

        public override bool Find(IMap map, IHeuristic heuristic)
        {

            if (Mutate == null || Crossover == null || Fitness == null || Selection == null)
                throw new System.Exception("GA cant run without all operators");

            if (Fitness.Heuristic == null)
                Fitness.Heuristic = heuristic;

            var Adaptation = new Adaptation(map);
            var rand = RandomFactory.Rand;
            var startNode = map.StartNode;
            var endNode = map.EndNode;

            for (int i = 0; i < PopulationSize; i++)
                Populations.Add(new Genome(map));


            CalcFitness();


            var step = 0;
            OnStart(BuildArgs(step, map));
            for (int i = 0; i < GenerationLimit; i++)
            {
                var newpopulations = new List<IGenome>();
                Populations = Populations.OrderBy(o => o.Fitness).ToList();
                for (int j = 0; j < BestSolutionToPick; j++)
                {
                    Populations[j].Fitness = Fitness.Calc(Populations[j]);
                    newpopulations.Add(Populations[j]);
                }
                var ran = rand.Next(1, Populations.Count);
                var best = Populations.First().ListNodes;
                var best2 = Selection.Select(Populations).ListNodes;
                endNode = best.Last();
                if (endNode.Equals(map.EndNode))
                {
                    endNode.Direction = DirectionMovement.None;
                    map.EndNode = endNode;

                    map.UpdateClosedList(new List<Node>());
                    map.UpdateOpenList(new List<Node>());


                    //if (!best.First().Equals(map.StartNode))
                    OnEnd(BuildArgs(step, map, true));
                    Generations = i;
                    return true;
                }
                map.UpdateClosedList(best);
                map.UpdateOpenList(best2);

                while (newpopulations.Count < Populations.Count)
                {
                    // Selection
                    var nodemom = Selection.Select(Populations);
                    var nodedad = Selection.Select(Populations);
                    // CrossOver
                    var cross = Crossover.Calc(new CrossoverOperation(nodemom, nodedad));

                    //if (!cross.Mom.ListNodes.First().Equals(map.StartNode)) { }
                    //if (!cross.Dad.ListNodes.First().Equals(map.StartNode)) { }

                    // Mutation
                    nodemom = Mutate.Calc(cross.Mom);
                    nodedad = Mutate.Calc(cross.Dad);

                    //if (!nodemom.ListNodes.First().Equals(map.StartNode)) { }
                    //if (!nodedad.ListNodes.First().Equals(map.StartNode)) { }

                    // Adaptation
                    nodemom = Adaptation.Calc(nodemom);
                    nodedad = Adaptation.Calc(nodedad);
                    nodemom.Fitness = Fitness.Calc(nodemom);
                    nodedad.Fitness = Fitness.Calc(nodedad);

                    //if (!nodemom.ListNodes.First().Equals(map.StartNode)) { }
                    //if (!nodedad.ListNodes.First().Equals(map.StartNode)) { }

                    // Add in new population
                    newpopulations.Add(nodemom);
                    newpopulations.Add(nodedad);
                }
                Populations = newpopulations.ToList();
                OnStep(BuildArgs(step++, map));
            }
            Generations = GenerationLimit;
            OnEnd(BuildArgs(step, map, false));
            return false;
        }
        void CalcFitness()
        {
            foreach (var item in Populations)
                item.Fitness = Fitness.Calc(item);
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