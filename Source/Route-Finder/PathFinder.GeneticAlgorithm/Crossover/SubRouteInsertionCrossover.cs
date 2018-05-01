using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.Routes;
using System.Linq;
namespace PathFinder.GeneticAlgorithm.Crossover
{
    public class SubRouteInsertionCrossover : AbstractCrossover, ICrossover
    {
        readonly IRouteService service;
        readonly IRandom _random;

        public SubRouteInsertionCrossover(GASettings settings, IRandom random, IRouteService service) : base(settings)
        {
            _random = random;
            this.service = service;
        }

        public override Genome[] Make(Genome mon, Genome dad)
        {
            if (_random.NextDouble() > CrossoverRate || mon.Equals(dad))
                return new[] { mon, dad };

            var son1 = GenerateSon(mon, dad);
            var son2 = GenerateSon(dad, mon);

            return new[] { son1, son2 };
        }


        Genome GenerateSon(Genome mon, Genome dad)
        {
            var usedRouteCount = mon.GetUsedTrucksCount;
            var routeIndexToBeUsed = _random.Next(0, usedRouteCount);

            var route = mon.Trucks[routeIndexToBeUsed];

            var qtdLocalsToPick = _random.Next(1, route.Locals.Count);

            var subRouteStartIndex = 0;
            do { subRouteStartIndex = _random.Next(0, route.Locals.Count); }
            while (subRouteStartIndex + qtdLocalsToPick - 1 >= route.Locals.Count);

            var subroute = route.Locals.Skip(subRouteStartIndex).Take(qtdLocalsToPick).ToList();

            var firstItemOfSR = subroute.First();

            var closestItem = (from t in mon.Trucks
                               from l in t.Locals
                               where !subroute.Contains(l)
                               let distance = service.GetRouteAsync(firstItemOfSR, l).Result.Metros
                               orderby distance
                               select l).First();

            //find in the dadRoute a location of the closest item
            var son = new Genome(dad);
            //removeSubroute of the son
            foreach (var truck in son.Trucks)
                truck.Locals = truck.Locals.Where(l => !subroute.Any(s => s.Equals(l))).ToList();

            var routeToInsert = son.Trucks.First(e => e.Locals.Any(l => l.Equals(closestItem)));
            var indexOfTheClosestItem = routeToInsert.Locals.IndexOf(closestItem) + 1;

            routeToInsert.Locals =
                routeToInsert
                .Locals
                .Take(indexOfTheClosestItem)
                .Concat(subroute)
                .Concat(routeToInsert.Locals.Skip(indexOfTheClosestItem))
                .ToList();

            son.ShrinkTruks();

            return son;

        }
    }
}
