using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.GeneticAlgorithm.Factories;
using PathFinder.Routes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathFinder.GeneticAlgorithm
{
    public class Genome : IGenome
    {
        public RouteMap Map { get; set; }
        public List<Node> ListNodes { get; set; }
        public List<Route> ListRoutes { get; set; }
        public double Fitness { get; set; }
        readonly SearchRoute Search = new SearchRoute();
        public Genome(RouteMap map)
        {
            Map = map;
            Initialize();
        }
        public Genome(IGenome genome)
        {
            Map = genome.Map;
            ListNodes = Copy(genome.ListNodes);
        }
        void Initialize()
        {
            ListNodes = new List<Node>();
            var rand = RandomFactory.Rand;

            var count = Map.Destinations.Count;

            while (count > 0)
            {
                var i = rand.Next(Map.Destinations.Count);

                if (ListNodes.Exists(o=>o.MapPoint.Name == Map.Destinations[i].Name))
                    continue;

                ListNodes.Add(new Node(Map.Destinations[i]));

                count--;
            }
        }
        public async Task CalcRoutesAsync()
        {
            var point = Map.Storage;
            ListRoutes = new List<Route>();

            Route route;
            foreach (var item in ListNodes)
            {
                route = await Search.GetRouteAsync(point, item.MapPoint);

                ListRoutes.Add(route);

                point = item.MapPoint;
            }

            //route = Search.GetRoute(point, Map.Storage);

            //ListRoutes.Add(route);
        }

        public void Save()
        {
            Search.SaveRouteImage(ListRoutes);
        }

        public bool IsEqual(IGenome genome)
        {
            if (ListNodes.Count != genome.ListNodes.Count)
                return false;
            for (int i = 0; i < ListNodes.Count; i++)
            {
                if (ListNodes[i] != genome.ListNodes[i])
                    return false;
            }
            return true;
        }
        private static List<Node> Copy(List<Node> listnode)
        {
            var returnnode = new List<Node>();
            foreach (var item in listnode)
                returnnode.Add(new Node(item));
            return returnnode;
        }
        public override string ToString()
        {
            return $"F={Fitness}";
        }
    }
}