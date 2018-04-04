using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PathFinder.Routes
{
    public class GoogleService : IRouteService
    {
        const string Url = "https://maps.googleapis.com/maps/api/";
        public string Key = "AIzaSyBm6unznpnoVDNak1s-iV_N9bQqCVpmKpE";

        readonly HttpClient httpClient;

        public GoogleService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        public virtual async Task<Rota> GetRouteAsync(Local origin, Local destination)
        {
            var request = GetRequestPointRoute(origin, destination);
            var ret = await ReadRequestRouteAsync(origin, destination, request);

            Console.WriteLine($"Rota Encontrada: : {origin.Endereco} -> {destination.Endereco} = {ret.Metros}m");

            return ret;
        }
        public async Task<Rota> ReadRequestRouteAsync(Local origin, Local destination, string url)
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

            return new Rota
            {
                Origem = origin,
                Destino = destination,
                Metros = distance,
                Segundos = duration
            };
        }

        public virtual async Task<Local> GetPointAsync(Local local)
        {

            var url = GetRequestAddress((local.Endereco));
            var ret = await ReadRequestPointAsync(local.Endereco, local.Name, url, local.Period);
            Console.WriteLine($"Endereço Encontrado: {ret.Endereco} ({ret.Latitude},{ret.Longitude})");

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


            return new Local(name, address) { Latitude = lat, Longitude = lon, Period = period };
        }


        string GetRequestNameRoute(Local ori, Local des)
            => $"{Url}directions/json?origin={(ori.Endereco)}&" +
                $"destination={(des.Endereco)}&sensor=false&key={Key}";

        string GetRequestPointRoute(Local ori, Local des)
            => $"{Url}directions/json?" +
                $"origin={ConvNumber(ori.Latitude)},{ConvNumber(ori.Longitude)}&" +
                $"destination={ConvNumber(des.Latitude)},{ConvNumber(des.Longitude)}&" +
                $"sensor=false&key={Key}";

        string GetRequestAddress(string address)
            => $"{Url}geocode/json?address={(address)}&sensor=false&key={Key}";


        string ConvNumber(double num)
            => Math.Round(num, 6).ToString().Replace(',', '.');

    }
}