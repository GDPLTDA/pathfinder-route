using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using RouteMatrix = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, CalcRoute.Routes.Rota>>;

namespace CalcRoute.Routes
{
    public class CachedGoogleMatrixService : GoogleMatrixService, ICachedRouteService
    {
        private ConcurrentDictionary<string, RouteMatrix> routes;
        private ConcurrentDictionary<string, Local> locals;
        public bool UseCache { get; set; } = true;
        public bool HasCache => !routes.IsEmpty;

        public CachedGoogleMatrixService(HttpClient httpClient) : base(httpClient)
        {
            routes = new ConcurrentDictionary<string, RouteMatrix>();
            locals = new ConcurrentDictionary<string, Local>();
        }

        public async override Task Prepare(IEnumerable<Local> locals)
        {
            var keyOfLocals = Encode(locals);

            if (UseCache && routes.TryGetValue(keyOfLocals, out var temp))
                routeMatrix = temp;
            else
            {
                await base.Prepare(locals);
                routes.AddOrUpdate(keyOfLocals, routeMatrix, (_, v) => routeMatrix);
            }

        }

        public async override Task<Local> GetPointAsync(Local local)
        {
            if (UseCache && locals.TryGetValue(local.Endereco, out var _local))
                return new Local
                {
                    Endereco = _local.Endereco,
                    Latitude = _local.Latitude,
                    Longitude = _local.Longitude,
                    Name = local.Name,
                    Period = local.Period
                };

            var point = await base.GetPointAsync(local);
            locals.AddOrUpdate(local.Endereco, point, (_, v) => point);

            return point;
        }

        public void SaveCache(string name = "Cache")
        {
            if (!UseCache)
                return;

            var jsonRoutes = JsonConvert.SerializeObject(routes);
            var jsonLocals = JsonConvert.SerializeObject(locals);

            File.WriteAllText($"Cache\\{name}_Route.txt", jsonRoutes);
            File.WriteAllText($"Cache\\{name}_Point.txt", jsonLocals);

        }

        public void LoadCache(string name = "Cache")
        {
            if (!UseCache)
                return;

            try
            {
                var routeFile = $"Cache\\{name}_Route.txt";
                if (File.Exists(routeFile))
                {
                    var jsonRoutes = File.ReadAllText(routeFile);
                    routes = JsonConvert.DeserializeObject<ConcurrentDictionary<string, RouteMatrix>>(jsonRoutes);
                }

                var localFile = $"Cache\\{name}_Point.txt";
                if (File.Exists(localFile))
                {
                    var jsonLocals = File.ReadAllText(localFile);
                    locals = JsonConvert.DeserializeObject<ConcurrentDictionary<string, Local>>(jsonLocals);
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public string GetRouteCache() => JsonConvert.SerializeObject(routes);
    }
}
