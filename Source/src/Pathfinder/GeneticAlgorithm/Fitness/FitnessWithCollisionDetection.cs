using Pathfinder.Abstraction;
using System.Linq;
using static System.Math;

namespace Pathfinder.Fitness
{
    public class FitnessWithCollisionDetection : IFitness
    {
        public FitnessWithCollisionDetection()
        {
            Penalty = Constants.PENALTY;
        }

        public IHeuristic Heuristic { get; set; }
        public double Penalty { get; set; }

        public double Calc(IGenome genome)
        {
            var _endNode = genome.Map.EndNode;
            var lastnode = genome.ListNodes.Last();
            var startnode = genome.ListNodes.First();
            var HeuristicMaxDistance = Heuristic.Calc(Abs(startnode.X - _endNode.X), Abs(startnode.Y - _endNode.Y));
            var HeuristicValue = Heuristic.Calc(Abs(lastnode.X - _endNode.X), Abs(lastnode.Y - _endNode.Y));
            var penalty = (double)0;
            if (lastnode.Collision)
                penalty = Penalty * (HeuristicValue / HeuristicMaxDistance);
            return penalty + HeuristicValue;
        }
    }
}
