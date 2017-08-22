using Pathfinder.Abstraction;
using System.Linq;
using static System.Math;

namespace Pathfinder.Fitness
{
    public class FitnessWithCirclicValidation : IFitness
    {
        public IHeuristic Heuristic { get; set; }
        public double Penalty { get; set; }

        public FitnessWithCirclicValidation()
        {
            Penalty = Constants.PENALTY;
        }

        public double Calc(IGenome genome)
        {
            var _endNode = genome.Map.EndNode;
            var lastnode = genome.ListNodes.Last();
            var startnode = genome.ListNodes.First();
            var HeuristicMaxDistance = Heuristic.Calc(Abs(startnode.X - _endNode.X), Abs(startnode.Y - _endNode.Y));
            var HeuristicValue = Heuristic.Calc(Abs(lastnode.X - _endNode.X), Abs(lastnode.Y - _endNode.Y));
            // verifica se tem caminho ciclico
            var IsCirclic = genome
                            .ListNodes
                            .GroupBy(e => new { e.X, e.Y })
                            .Select(e => new { e.Key.X, e.Key.Y, qtd = e.Count() })
                            .Any(e => e.qtd > 1);
            double penalty = 0;
            if (IsCirclic)
                penalty = Penalty * (HeuristicValue / HeuristicMaxDistance); // calcula proporção da distancia, caso esteja mais lonje o peso do caminho ciclico é maior
            return penalty + HeuristicValue;
        }
    }
}
