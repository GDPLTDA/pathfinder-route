using Pathfinder.Abstraction;
using System.Linq;
using static System.Math;

namespace Pathfinder.Fitness
{
    public class FitnessHeuristic : IFitness
    {
        public IHeuristic Heuristic { get; set; }
        public double Penalty { get; set; }

        public FitnessHeuristic()
        {

        }

        public double Calc(IGenome genome)
        {
            var _endNode = genome.Map.EndNode;
            var lastnode = genome.ListNodes.Last();
            return Heuristic.Calc(Abs(lastnode.X - _endNode.X), Abs(lastnode.Y - _endNode.Y));
        }
    }
}