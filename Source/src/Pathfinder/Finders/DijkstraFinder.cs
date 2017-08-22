using Pathfinder.Abstraction;

namespace Pathfinder.Finders
{
    public class DijkstraFinder : AStarFinder
    {
        public DijkstraFinder() : base()
        {
            Name = "Dijkstra";
        }
        public override double CalcH(IHeuristic h, int dx, int dy) => 0.0f;
    }
}