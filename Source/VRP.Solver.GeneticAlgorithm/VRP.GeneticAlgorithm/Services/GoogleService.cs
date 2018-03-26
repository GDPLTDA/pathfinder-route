using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VRP.GeneticAlgorithm.Models;

namespace VRP.GeneticAlgorithm
{
    public class GoogleService : IRouteService
    {
        const string Url = "https://maps.googleapis.com/maps/api/";
        const string Key = "AIzaSyBFP8cY4DSZM_7Z9k2svtu-Ktdjhq23UNI";

        readonly HttpClient httpClient;

        public GoogleService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }



        public async Task<IReadOnlyCollection<Route>> CalcFullRoute(IEnumerable<Local> locals) =>
            await
                locals
                .ToObservable()
                .Select(e => (e, e))
                .Scan((a, b) => (a.Item2, b.Item1))
                .Skip(1)
                .SelectMany(e => Observable.FromAsync(x => GetRouteAsync(e.Item2, e.Item1)))
                .ToArray();


        public virtual async Task<Route> GetRouteAsync(Local origin, Local destination)
        {
            var request = GetRequestPointRoute(origin, destination);
            var ret = await ReadRequestRouteAsync(origin, destination, request);

            Console.WriteLine($"Rota Encontrada: : {origin.Address} -> {destination.Address} = {ret.Meters}m");

            return ret;
        }
        public async Task<Route> ReadRequestRouteAsync(Local origin, Local destination, string url)
        {
            var json = await httpClient.GetStringAsync(url);
            dynamic data = JsonConvert.DeserializeObject(json);

            if (data == null)
                throw new Exception("Request nao pode ser realizado!");

            var routes = data.routes as IEnumerable<dynamic>;
            if (!routes.Any())
            {
                if (data.status == "OVER_QUERY_LIMIT")
                    throw new Exception("Estourou o limite diário!");

            }

            var lastRoute = routes?.LastOrDefault();
            var route = (lastRoute?.legs as IEnumerable<dynamic>)?.LastOrDefault();

            double distance = route.distance.value;
            double duration = route.duration.value;

            return new Route(origin, destination, distance, duration);
        }

        public virtual async Task<Local> GetPointAsync(string address, string name)
        {

            var url = GetRequestAddress((address));
            var ret = await ReadRequestPointAsync(address, name, url, Period.Default);
            Console.WriteLine($"Endereço Encontrado: {ret.Address} ({ret.Position.Lat},{ret.Position.Long})");

            return ret;
        }
        public async Task<Local> ReadRequestPointAsync(string address, string name, string url, Period period)
        {
            var json = await httpClient.GetStringAsync(url);
            dynamic data = JsonConvert.DeserializeObject(json);

            if (data == null || data.status == "ZERO_RESULTS")
                throw new Exception("Dados nao encontrados");

            var results = data.results as IEnumerable<dynamic>;

            if (!results.Any())
                Console.WriteLine($"{data.status}: {address}");

            var r = results.LastOrDefault();
            double lat = r.geometry.location.lat;
            double lon = r.geometry.location.lng;

            var position = new Position(lat, lon);

            return new Local(name, address, position, period);
        }


        static string GetRequestNameRoute(Local ori, Local des)
            => $"{Url}directions/json?origin={(ori.Address)}&" +
                $"destination={(des.Address)}&sensor=false&key={Key}";

        static string GetRequestPointRoute(Local ori, Local des)
            => $"{Url}directions/json?" +
                $"origin={ConvNumber(ori.Position.Lat)},{ConvNumber(ori.Position.Long)}&" +
                $"destination={ConvNumber(des.Position.Lat)},{ConvNumber(des.Position.Long)}&" +
                $"sensor=false&key={Key}";

        static string GetRequestAddress(string address)
            => $"{Url}geocode/json?address={(address)}&sensor=false&key={Key}";


        static string ConvNumber(double num)
            => Math.Round(num, 6).ToString().Replace(',', '.');

    }
}