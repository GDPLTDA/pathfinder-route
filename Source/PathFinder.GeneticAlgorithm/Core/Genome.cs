using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.GeneticAlgorithm.Factories;
using PathFinder.Routes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PathFinder.GeneticAlgorithm
{
    public class Genome : IGenome
    {
        public RouteMap Map { get; set; }
        public List<MapPoint> ListPoints { get; set; }
        public List<Route> ListRoutes { get; set; }
        public double Fitness { get; private set; }

        public Genome()
        {

        }
        public Genome(RouteMap map)
        {
            Map = map;
            Initialize();
        }
        public Genome(IGenome genome)
        {
            Map = genome.Map;
            ListPoints = genome.ListPoints.Select(o => o).ToList();
            ListRoutes = genome.ListRoutes.Select(o => o).ToList();
        }
        void Initialize()
        {
            ListPoints = new List<MapPoint>();
            var rand = RandomFactory.Rand;

            var count = Map.Destinations.Count;

            while (count > 0)
            {
                var i = rand.Next(Map.Destinations.Count);

                if (ListPoints.Exists(o => o.Equals(Map.Destinations[i])))
                    continue;

                ListPoints.Add(Map.Destinations[i]);

                count--;
            }
        }
        public async Task CalcRoutesAsync()
        {
            var point = Map.Storage;
            ListRoutes = new List<Route>();
            ListPoints = Map.Destinations;
            Route route;
            foreach (var item in ListPoints)
            {
                route = await SearchRoute.GetRouteAsync(point, item);
                ListRoutes.Add(route);

                point = item;
            }
            route = await SearchRoute.GetRouteAsync(ListPoints.Last(), Map.Storage);
            ListRoutes.Add(route);
        }

        public void Save() =>
            SearchRoute.SaveRouteImage(ListRoutes);

        public bool IsEqual(IGenome genome)
        {
            if (genome != null)
                if (ListRoutes.Sum(o => o.Meters) == genome.ListRoutes.Sum(o => o.Meters))
                    if (ListRoutes.Sum(o => o.Minutes) == genome.ListRoutes.Sum(o => o.Minutes))
                        return true;

            return false;
        }
        public override string ToString() => $"F={Fitness}";

        public static IGenome Generator(RouteMap map)
        {
            return new Genome(map);
        }

        public void CalcFitness(IFitness fitness)
        {
            Fitness = fitness.Calc(this);
        }
    }
}