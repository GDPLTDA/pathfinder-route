using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.GeneticAlgorithm.Factories;
using PathFinder.Routes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace PathFinder.GeneticAlgorithm
{
    public class Genome : IGenome
    {
        public RouteMap Map { get; set; }
        public List<Node> ListNodes { get; set; }
        public List<Route> ListRoutes { get; set; }
        public double Fitness { get; private set; }
        public DateTime Finish { get; set; }

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

                if (ListNodes.Exists(o=>o.MapPoint.Equals(Map.Destinations[i])))
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
                route = await SearchRoute.GetRouteAsync(point, item.MapPoint);

                ListRoutes.Add(route);

                point = item.MapPoint;
            }
            route = await SearchRoute.GetRouteAsync(point, Map.Storage);
            ListRoutes.Add(route);
        }

        public void Save()
        {
            SearchRoute.SaveRouteImage(ListRoutes);
        }

        public bool IsEqual(IGenome genome)
        {
            if(genome!=null)
                if(ListRoutes.Sum(o=>o.Meters) == genome.ListRoutes.Sum(o => o.Meters))
                    if (ListRoutes.Sum(o => o.Minutes) == genome.ListRoutes.Sum(o => o.Minutes))
                        return true;

            return false;
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

        public static IEnumerable<IGenome> Generator(RouteMap map)
        {
            while (true)
                yield return new Genome(map);
        }

        public void CalcFitness(IFitness fitness)
        {
            Fitness = fitness.Calc(this);
        }
    }
}