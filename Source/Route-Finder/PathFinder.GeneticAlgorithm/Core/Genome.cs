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
        public IList<Local> Locals { get; set; }
        public IList<Truck> Trucks { get; set; }
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
            Locals = genome.Locals.ToList();
            Trucks = genome.Trucks.ToList();
        }
        void Initialize()
        {
            Locals = new List<Local>();
            var rand = RandomFactory.Rand;

            var count = Map.Destinations.Count;

            while (count > 0)
            {
                var i = rand.Next(Map.Destinations.Count);

                if (Locals.Any(o => o.Equals(Map.Destinations[i])))
                    continue;

                Locals.Add(Map.Destinations[i]);

                count--;
            }
        }
        public async Task CalcRoutesAsync(IRouteService routeService) =>
            await Trucks
                    .Select(t => t.CalcRoutesAsync(routeService, Map.Depot, Map.Destinations))
                    .WhenAllAsync();

        public bool IsEqual(IGenome genome) =>
            Trucks.Sum(t => t.GetTotalMeters()) == genome?.Trucks.Sum(t => t.GetTotalMeters()) &&
            Trucks.Sum(t => t.GetTotalMinutes()) == genome?.Trucks.Sum(t => t.GetTotalMinutes())
        ;

        public override string ToString() => $"F={Fitness}";

        public static IGenome Generator(Roteiro map) => new Genome(map);

        public void CalcFitness(IFitness fitness)
        {
            Fitness = fitness.Calc(this);
        }
    }
}