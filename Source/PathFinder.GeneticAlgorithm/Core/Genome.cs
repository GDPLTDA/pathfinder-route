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
        public Roteiro Map { get; set; }
        public List<Local> ListPoints { get; set; }
        public List<Rota> ListRoutes { get; set; }
        public double Fitness { get; private set; }

        public Genome()
        {

        }
        public Genome(Roteiro map)
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
            ListPoints = new List<Local>();
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
            ListRoutes = new List<Rota>();
            ListPoints = Map.Destinations;
            Rota route;
            foreach (var item in ListPoints)
            {
                route = await SearchRoute.GetRouteAsync(point, item);
                ListRoutes.Add(route);

                point = item;
            }

            Local lastpoint;

            if (ListPoints.Any())
                lastpoint = ListPoints.Last();
            else
                lastpoint = point;

            route = await SearchRoute.GetRouteAsync(lastpoint, Map.MainStorage);
            ListRoutes.Add(route);
        }

        public void Save() =>
            SearchRoute.SaveRouteImage(ListRoutes);

        public bool IsEqual(IGenome genome)
        {
            if (genome != null)
                if (ListRoutes.Sum(o => o.Metros) == genome.ListRoutes.Sum(o => o.Metros))
                    if (ListRoutes.Sum(o => o.Minutos) == genome.ListRoutes.Sum(o => o.Minutos))
                        return true;

            return false;
        }
        public override string ToString() => $"F={Fitness}";

        public static IGenome Generator(Roteiro map)
        {
            return new Genome(map);
        }

        public void CalcFitness(IFitness fitness)
        {
            Fitness = fitness.Calc(this);
        }
    }
}