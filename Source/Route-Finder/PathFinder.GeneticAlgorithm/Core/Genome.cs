using CalcRoute.GeneticAlgorithm.Abstraction;
using CalcRoute.Routes;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace CalcRoute.GeneticAlgorithm
{
    public class Genome
    {
        public Roteiro Map { get; set; }
        public IList<Truck> Trucks { get; set; }

        public double Fitness { get; private set; }


        public GASettings Settings { get; }

        public int GetUsedTrucksCount => Trucks.Count(e => e.Locals.Any());


        public Genome(Roteiro map, GASettings settings)
        {
            Map = map;
            Settings = settings;
            Initialize();
        }
        public Genome(Genome genome)
        {
            Map = genome.Map;
            Settings = genome.Settings;
            Trucks = genome.Trucks.ToList();
            ShrinkTruks();
        }
        void Initialize()
        {
            var rand = RandomSingleton.Instance;
            Trucks = Enumerable.Range(0, Settings.NumberOfTrucks)
                       .Select(e => new Truck { Id = e })
                       .ToList();

            var randomLocals =
                    new Stack<Local>(Map.Destinations.Shuffle());

            while (randomLocals.Any())
            {
                var truckId = rand.Next(0, Settings.NumberOfTrucks);
                Trucks[truckId].Locals.Add(randomLocals.Pop());
            }

            ShrinkTruks();
        }

        public void ShrinkTruks() =>
            Trucks = Trucks
                        .OrderByDescending(t => t.Locals.Any())
                        .ThenBy(t => t.Id)
                        .Select((t, i) => new Truck(t.Locals) { Id = i })
                        .ToList();


        public async Task CalcRoutesAsync(IRouteService routeService) =>
            await Trucks
                    .Select(t => t.CalcRoutesAsync(routeService, Map.Depot))
                    .WhenAllAsync();

        public bool IsEqual(Genome genome) =>
            Trucks.Sum(t => t.GetTotalMeters()) == genome?.Trucks.Sum(t => t.GetTotalMeters()) &&
            Trucks.Sum(t => t.GetTotalMinutes()) == genome?.Trucks.Sum(t => t.GetTotalMinutes())
        ;

        public override string ToString() => $"F={Fitness}";

        public static Genome Generator(Roteiro map, GASettings settings) => new Genome(map, settings);

        public void CalcFitness(IFitness fitness) => Fitness = fitness.Calc(this, Settings);
    }
}