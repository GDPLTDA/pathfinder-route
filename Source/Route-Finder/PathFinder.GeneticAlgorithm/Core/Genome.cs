using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.GeneticAlgorithm.Core;
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
        public TruckCollection Locals { get; set; }

        public double Fitness { get; private set; }


        public GASettings Settings { get; }

        public Genome() { }
        public Genome(Roteiro map, GASettings settings)
        {
            Map = map;
            Settings = settings;
            Initialize();
        }
        public Genome(IGenome genome)
        {
            Map = genome.Map;
            Settings = genome.Settings;
            Locals = new TruckCollection(genome.Locals.Trucks);
        }
        void Initialize()
        {
            var rand = RandomFactory.Rand;
            var trucks = Enumerable.Range(0, Settings.NumberOfTrucks)
                       .Select(e => new Truck { Id = e })
                       .ToList();

            var randomLocals =
                    new Stack<Local>(Map.Destinations.Shuffle());

            while (randomLocals.Any())
            {
                var truckId = rand.Next(0, Settings.NumberOfTrucks);
                trucks[truckId].Locals.Add(randomLocals.Pop());
            }

            Locals = new TruckCollection(trucks);
        }

        public async Task CalcRoutesAsync(IRouteService routeService) =>
            await Locals.Trucks
                    .Select(t => t.CalcRoutesAsync(routeService, Map.Depot))
                    .WhenAllAsync();

        public bool IsEqual(IGenome genome) =>
            Locals.Trucks.Sum(t => t.GetTotalMeters()) == genome?.Locals.Trucks.Sum(t => t.GetTotalMeters()) &&
            Locals.Trucks.Sum(t => t.GetTotalMinutes()) == genome?.Locals.Trucks.Sum(t => t.GetTotalMinutes())
        ;

        public override string ToString() => $"F={Fitness}";

        public static IGenome Generator(Roteiro map, GASettings settings) => new Genome(map, settings);

        public void CalcFitness(IFitness fitness) => Fitness = fitness.Calc(this, Settings);
    }
}