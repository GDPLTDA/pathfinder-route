using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CalcRoute.Routes
{
    public class CachedGoogleDirectionsService : GoogleDirectionsService, IRouteService
    {
        private ConcurrentDictionary<string, Rota> routes;
        private ConcurrentDictionary<string, Local> locals;
        public bool UseCache { get; set; } = true;

        public CachedGoogleDirectionsService(HttpClient httpClient) : base(httpClient)
        {
            routes = new ConcurrentDictionary<string, Rota>();
            locals = new ConcurrentDictionary<string, Local>();
        }

        public async override Task<Rota> GetRouteAsync(Local origin, Local destination)
        {
            var key = $"{origin.Latitude},{origin.Longitude}->{destination.Latitude},{destination.Longitude}";

            if (routes.TryGetValue(key, out var route))
                return route;


            var value = await base.GetRouteAsync(origin, destination);
            routes.AddOrUpdate(key, value, (_, v) => value);

            return value;
        }
        public async override Task<Local> GetPointAsync(Local local)
        {
            if (locals.TryGetValue(local.Endereco, out var _local))
                return _local;

            var point = await base.GetPointAsync(local);
            locals.AddOrUpdate(local.Endereco, point, (_, v) => point);

            return point;
        }

        public void SaveCache()
        {
            if (!UseCache)
                return;


            var jsonRoutes = JsonConvert.SerializeObject(routes);
            var jsonLocals = JsonConvert.SerializeObject(locals);

            File.WriteAllText($"RouteCache", jsonRoutes);
            File.WriteAllText($"PointCache", jsonLocals);

        }

        public void LoadCache()
        {
            if (!UseCache)
                return;

            var routeFile = $"RouteCache.txt";
            if (File.Exists(routeFile))
            {
                var jsonRoutes = File.ReadAllText(routeFile);
                routes = JsonConvert.DeserializeObject<ConcurrentDictionary<string, Rota>>(jsonRoutes);
            }

            var localFile = $"PointCache.txt";
            if (File.Exists(localFile))
            {
                var jsonLocals = File.ReadAllText(localFile);
                locals = JsonConvert.DeserializeObject<ConcurrentDictionary<string, Local>>(jsonLocals);
            }

        }


    }
}
