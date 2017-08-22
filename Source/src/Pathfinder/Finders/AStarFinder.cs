using Pathfinder.Abstraction;
using static System.Math;
namespace Pathfinder.Finders
{
    public class AStarFinder : AbstractFinder
    {
        public AStarFinder() : base("A* (A Star)")
        {
            SleepUITimeInMs = 200;

        }

        public virtual double CalcH(IHeuristic h, int dx, int dy)
        {
            return h.Calc(dx, dy);
        }

        public override bool Find(IMap grid, IHeuristic heuristic)
        {
            Clear();
            var sqrt2 = Sqrt(2);
            grid.StartNode.Cost = 0;
            grid.EndNode.Cost = 0;
            grid.AddInOpenList(grid.StartNode);
            var step = 0;
            OnStart(BuildArgs(step, grid));
            while (grid.OpenListCount() != 0)
            {
                var node = grid.PopOpenList();
                grid.AddInClosedList(node);
                if (node == grid.EndNode)
                {
                    //_endNode = node;
                    OnEnd(BuildArgs(step, grid, true));
                    return true;
                }
                var neighbors = grid.GetNeighbors(node);
                for (var i = 0; i < neighbors.Count; ++i)
                {
                    var neighbor = neighbors[i];
                    if (grid.IsClosed(neighbor))
                        continue;

                    var x = neighbor.X;
                    var y = neighbor.Y;
                    // get the distance between current node and the neighbor
                    // and calculate the next g score
                    var ng = node.G + ((x - node.X == 0 || y - node.Y == 0) ? 1 : sqrt2);
                    // check if the neighbor has not been inspected yet, or
                    // can be reached with smaller cost from the current node
                    if (!grid.IsOpen(neighbor) || ng < neighbor.G)
                    {
                        neighbor.G = ng;
                        neighbor.H = Weight * CalcH(heuristic, Abs(x - grid.EndNode.X), Abs(y - grid.EndNode.Y));
                        neighbor.Cost = neighbor.G + neighbor.H;
                        neighbor.ParentNode = node;
                        if (!grid.IsOpen(neighbor))
                            grid.PushInOpenList(neighbor);
                    }
                }
                grid.OrderOpenList(e => e.Cost);
                OnStep(BuildArgs(step++, grid));
            }
            OnEnd(BuildArgs(step, grid, false));
            return false;
        }
    }
}