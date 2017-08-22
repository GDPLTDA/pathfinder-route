using Pathfinder.Abstraction;
using Pathfinder.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace Pathfinder.MapGenerators
{
    public class StandardMapGenerator : IMapGenerator
    {
        public List<Node> GridMap { get; set; } = new List<Node>();
        public int Blocksize;

        public IMap DefineMap(DiagonalMovement diagonal, int width, int height, int seed, int minPathLength)
        {

            int _GDC(int a, int b) => (b == 0 || a == 0) ? a | b : _GDC(Min(a, b), Max(a, b) % Min(a, b));
            var blocksize = Blocksize > 0 ? Blocksize : _GDC(width, height);
            var IsAGoodMap = false;
            IMap ret = null;

            // finder para valida se o mapa é passavel
            var aStar = FinderFactory.GetAStarImplementation();
            IHeuristic heuristic;
            heuristic = diagonal == DiagonalMovement.Never ?
                            HeuristicFactory.GetManhattamImplementation() :
                            HeuristicFactory.GetOctileImplementation();

            var subgrid = new List<Node>();
            while (!IsAGoodMap)
            {
                var nodes = new List<Node>();
                var _map = new Map(diagonal, width, height);

                var size = Convert.ToInt32(blocksize * blocksize * (seed / 100M));
                var rand = new Random();
                while (size > 0)
                {
                    var p = RandNode(rand, blocksize, blocksize, true);
                    subgrid.Add(p);
                    size--;
                }
                for (int i = 0; i < width; i += blocksize)
                {
                    for (int j = 0; j < height; j += blocksize)
                    {
                        foreach (var item in subgrid)
                        {
                            var x = item.X + i;
                            var y = item.Y + j;
                            if (x < width && y < height)
                            {
                                var node = new Node(x, y, false);
                                GridMap.Add(node);
                            }
                        }
                    }
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
                        _map.Clear();
                        ret = _map;
                    }
                }
                GridMap = new List<Node>();
                subgrid = new List<Node>();
            }

            ret.Clear();
            ret.MapType = MapGeneratorEnum.WithPattern;
            return ret;
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

        public IMap DefineMap()
        {
            throw new NotImplementedException();
        }
    }
}