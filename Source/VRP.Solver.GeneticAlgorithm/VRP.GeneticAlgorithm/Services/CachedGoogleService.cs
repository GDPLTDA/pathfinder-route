using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using VRP.GeneticAlgorithm.Models;

namespace VRP.GeneticAlgorithm
{
    public class CachedGoogleService : GoogleService, IRouteService
    {
        private ConcurrentDictionary<string, Route> routes;
        private ConcurrentDictionary<string, Local> locals;

        public CachedGoogleService(HttpClient httpClient) : base(httpClient)
        {
            routes = new ConcurrentDictionary<string, Route>();
            locals = new ConcurrentDictionary<string, Local>();
        }

        public async override Task<Route> GetRouteAsync(Local origin, Local destination)
        {
            var key = $"{origin.Position.Lat},{origin.Position.Long}->{destination.Position.Lat},{destination.Position.Long}";

            if (routes.TryGetValue(key, out var route))
                return route;


            var value = await base.GetRouteAsync(origin, destination);
            routes.AddOrUpdate(key, value, (_, v) => value);

            return value;
        }

        public async override Task<Local> GetPointAsync(string address, string name)
        {
            if (locals.TryGetValue(address, out var local))
                return local;

            var point = await base.GetPointAsync(address, name);
            locals.AddOrUpdate(address, point, (_, v) => point);

            return point;
        }

        public void Save(string fileName)
        {
            var jsonRoutes = JsonConvert.SerializeObject(routes);
            var jsonLocals = JsonConvert.SerializeObject(locals);

            File.WriteAllText($"routes-{fileName}", jsonRoutes);
            File.WriteAllText($"locals-{fileName}", jsonLocals);

        }

        public void Load(string fileName)
        {
            var jsonRoutes = File.ReadAllText($"routes-{fileName}");
            var jsonLocals = File.ReadAllText($"locals-{fileName}");

            routes = JsonConvert.DeserializeObject<ConcurrentDictionary<string, Route>>(jsonRoutes);
            locals = JsonConvert.DeserializeObject<ConcurrentDictionary<string, Local>>(jsonLocals);


        }


    }
}
