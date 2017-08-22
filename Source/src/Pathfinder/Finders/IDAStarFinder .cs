using Pathfinder.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
namespace Pathfinder.Finders
{
    public class IDAStarFinder : AbstractFinder
    {
        public bool TrackRecursion;
        public double TimeLimit;
        int nodesVisited;
        public IDAStarFinder() : base("IDA* (IDA Star)")
        {

            SleepUITimeInMs = 30;
            TimeLimit = double.PositiveInfinity;
            nodesVisited = 0;
            TrackRecursion = true;

        }
        private static double H(IHeuristic h, Node a, Node b)
        {
            return h.Calc(Abs(b.X - a.X), Abs(b.Y - a.Y));
        }
        private static double Cost(Node a, Node b)
        {
            return (a.X == b.X || a.Y == b.Y) ? 1 : Sqrt(2);
        }

        public override void StepConfig(IMap map)
        {
            if (map == null || !TrackRecursion)
                return;

            map.UpdateOpenList(new List<Node>());
            for (int i = 0; i < map.Height; i++)
                for (int j = 0; j < map.Width; j++)
                    if (map[i, j].Tested)
                        map.AddInOpenList(map[i, j]);
        }
        private Tuple<Node, double> Search(IHeuristic h, IMap map, Node node, double g, double cutoff, Dictionary<int, Node> route, int depth, Node end, int k)
        {
            nodesVisited++;
            // Enforce timelimit:
            if (_stopwatch.ElapsedMilliseconds > 0 &&
                _stopwatch.ElapsedMilliseconds > TimeLimit)
            {
                // Enforced as "path-not-found".
                return null;
            }
            var f = g + H(h, node, end) * Weight;
            // We've searched too deep for this iteration.
            if (f > cutoff)
            {
                return new Tuple<Node, double>(null, f); ;
            }
            if (node == end)
            {
                if (route.ContainsKey(depth))
                    route[depth] = node;
                else
                    route.Add(depth, node);
                return new Tuple<Node, double>(node, 0);
            }
            var neighbours = map.GetNeighbors(node);
            var min = double.PositiveInfinity;
            Tuple<Node, double> t;
            Node neighbour;
            var x = 0;
            for (x = 0; x < neighbours.Count; x++)
            {
                neighbour = neighbours[x];
                if (TrackRecursion)
                {
                    // Retain a copy for visualisation. Due to recursion, this
                    // node may be part of other paths too.
                    neighbour.RetainCount = neighbour.RetainCount + 1;
                    if (!neighbour.Tested)
                        neighbour.Tested = true;
                    OnStep(BuildArgs(k, map));
                }
                t = Search(h, map, neighbour, g + Cost(node, neighbour), cutoff, route, depth + 1, end, k);
                if (t == null)
                    return null;
                if (t.Item1 != null)
                {
                    if (route.ContainsKey(depth))
                        route[depth] = node;
                    else
                        route.Add(depth, node);
                    return t;
                }
                // Decrement count, then determine whether it's actually closed.
                if (TrackRecursion && (--neighbour.RetainCount) == 0)
                {
                    neighbour.Tested = false;
                }
                if (t.Item2 < min)
                {
                    min = t.Item2;
                }
            }
            return new Tuple<Node, double>(null, min);
        }
        public override bool Find(IMap grid, IHeuristic h)
        {
            Clear();
            var sqrt2 = Sqrt(2);
            var cutOff = H(h, grid.StartNode, grid.EndNode);
            Dictionary<int, Node> route;
            Tuple<Node, double> t;
            OnStart(BuildArgs(0, grid));
            var k = 0;
            for (k = 0; true; k++)
            {
                route = new Dictionary<int, Node>();
                t = Search(h, grid, grid.StartNode, 0, cutOff, route, 0, grid.EndNode, k);
                if (t == null || t.Item2 == double.PositiveInfinity)
                {
                    OnEnd(BuildArgs(k, grid, false));
                    return false;
                }
                if (t.Item1 != null)
                {
                    var lis = route.OrderByDescending(e => e.Key).Select(e => e.Value).ToList();
                    for (int i = 1; i < lis.Count; i++)
                    {
                        lis[i - 1].ParentNode = lis[i];
                    }
                    OnEnd(BuildArgs(k, grid, true));
                    return true;
                }
                cutOff = t.Item2;
            }
            // OnEnd(BuildArgs(0, false));
            // return false;
        }

    }
}