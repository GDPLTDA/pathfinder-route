using Pathfinder.Abstraction;
namespace Pathfinder.Finders
{
    public class BestFirstSearchFinder : AStarFinder
    {
        public BestFirstSearchFinder() : base()
        {
            Name = "Best First Search";
        }
        public override double CalcH(IHeuristic h, int dx, int dy) => base.CalcH(h, dx, dy) * 1000000;

    }
}