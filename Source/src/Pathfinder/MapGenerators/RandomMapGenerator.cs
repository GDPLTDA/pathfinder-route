using Pathfinder.Abstraction;
using Pathfinder.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Pathfinder.MapGenerators
{
    public class RandomMapGenerator : IMapGenerator
    {
        public List<Node> GridMap = new List<Node>();
        public IMap DefineMap(DiagonalMovement diagonal, int width, int height, int seed, int minPathLength)
        {


            var IsAGoodMap = false;
            IMap ret = null;

            var aStar = FinderFactory.GetAStarImplementation();
            IHeuristic heuristic;
            heuristic = diagonal == DiagonalMovement.Never ?
                            HeuristicFactory.GetManhattamImplementation() :
                            HeuristicFactory.GetOctileImplementation();


            while (!IsAGoodMap)
            {
                var nodes = new List<Node>();
                var _map = new Map(diagonal, width, height);

                var size = Convert.ToInt32((width * height) * (seed / 100M));
                var rand = new Random();

                while (size > 0)
                {
                    var p = RandNode(rand, width, height, true);
                    GridMap.Add(p);
                    size--;
                }
                _map.DefineAllNodes(GridMap);
                _map.StartNode = RandNode(rand, width, height, false);
                _map.EndNode = RandNode(rand, width, height, false);
                if (!_map.ValidMap())
                    throw new Exception("Invalid map configuration");
                if (aStar.Find(_map, heuristic)) // verifica se o mapa possui um caminho
                {
                    var path = _map.GetPath();
                    if (path.Max(e => e.G) >= minPathLength) // verifica se o caminho sastifaz o tamanho minimo
                    {
                        IsAGoodMap = true;
                        ret = _map;
                    }
                }
                GridMap = new List<Node>();
            }
            ret.Clear();
            ret.MapType = MapGeneratorEnum.Random;
            return ret;
        }

        public IMap DefineMap()
        {
            throw new NotImplementedException();
        }

        private Node RandNode(Random rand, int width, int height, bool wall)
        {
            Node p = null;
            while (p == null || GridMap.Exists(i => i.Equals(p)))
            {
                var x = rand.Next(0, width);
                var y = rand.Next(0, height);
                p = new Node(x, y, !wall, DirectionMovement.None);
            }
            return p;
        }
    }
}