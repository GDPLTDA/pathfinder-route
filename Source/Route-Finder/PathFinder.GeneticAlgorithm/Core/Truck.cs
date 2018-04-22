using PathFinder.Routes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PathFinder.GeneticAlgorithm
{
    public class Truck
    {
        public Truck()
        {
        }

        public int Id { get; set; }
        public IList<Rota> Routes { get; set; }
        public async Task CalcRoutesAsync(IRouteService routeService, Local depot, IReadOnlyCollection<Local> locals)
        {
            var point = depot;
            Routes = new List<Rota>();
            Rota route;

            foreach (var item in locals)
            {
                route = await routeService.GetRouteAsync(point, item);
                Routes.Add(route);

                point = item;
            }
        }

        public double GetTotalMeters() => Routes.Sum(o => o.Metros);
        public double GetTotalMinutes() => Routes.Sum(o => o.Minutos);

    }
}
