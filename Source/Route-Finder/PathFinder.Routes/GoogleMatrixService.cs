using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace CalcRoute.Routes
{
    public class GoogleMatrixService : IRouteService
    {

        protected readonly string Url = "https://maps.googleapis.com/maps/api/";
        protected readonly string Key = "AIzaSyBm6unznpnoVDNak1s-iV_N9bQqCVpmKpE";

        protected readonly HttpClient httpClient;
        protected Dictionary<string, Dictionary<string, Rota>> routeMatrix;

        public GoogleMatrixService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        public virtual async Task Prepare(IEnumerable<Local> locals)
        {
            routeMatrix = new Dictionary<string, Dictionary<string, Rota>>();
            var bufferedLocals = locals.Buffer(5).ToArray();

            for (int b = 0; b < bufferedLocals.Length; b++)
                for (int c = 0; c < bufferedLocals.Length; c++)
                {
                    var bufferOrigin = bufferedLocals[b];
                    var bufferDest = bufferedLocals[c];

                    var url = GetRequestMatrixUrl(bufferOrigin, bufferDest);
                    dynamic data = JsonConvert.DeserializeObject(await httpClient.GetStringAsync(url));

                    if (data.status == "OVER_QUERY_LIMIT")
                        throw new Exception("Estourou o limite diário!");


                    var rows = data.rows;
                    for (int i = 0; i < rows.Count; i++)
                    {
                        var cols = rows[i].elements;
                        var destinos = new Dictionary<string, Rota>();
                        for (int j = 0; j < cols.Count; j++)
                        {
                            var col = cols[j];
                            double metros = col.distance.value;
                            double segundos = col.duration.value;

                            var rotaBase = new Rota
                            {
                                Origem = bufferOrigin[i],
                                Destino = bufferDest[j],
                                Metros = metros,
                                Segundos = segundos
                            };

                            destinos.Add(ParseLocal(bufferDest[j]), rotaBase);

                        }

                        var key = ParseLocal(bufferOrigin[i]);
                        if (routeMatrix.ContainsKey(key))
                        {
                            var dict = routeMatrix[key];
                            foreach (var r in destinos)
                                dict.Add(r.Key, r.Value);
                        }
                        else
                            routeMatrix.Add(key, destinos);
                    }

                }
        }


        public virtual Task<Rota> GetRouteAsync(Local origin, Local destination)
        {
            if (routeMatrix == null || routeMatrix.Count == 0)
                throw new Exception("No initialized data");

            try
            {
                var rota = routeMatrix[ParseLocal(origin)][ParseLocal(destination)];

                return Task.FromResult(new Rota
                {
                    Origem = rota.Origem,
                    Destino = rota.Destino,
                    Metros = rota.Metros,
                    Segundos = rota.Segundos
                });
            }
            catch (Exception)
            {

                throw;
            }

        }

        public virtual async Task<Local> GetPointAsync(Local local)
        {

            var url = GetRequestAddress((local.Endereco));
            var ret = await ReadRequestPointAsync(local.Endereco, local.Name, url, local.Period);
            //Console.WriteLine($"Endereço Encontrado: {ret.Endereco} ({ret.Latitude},{ret.Longitude})");
            return ret;

        }

        public virtual async Task<Local> ReadRequestPointAsync(string address, string name, string url, Period period)
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

        string GetRequestAddress(string address)
                   => $"{Url}geocode/json?address={(address)}&sensor=false&key={Key}";


        string GetRequestMatrixUrl(IEnumerable<Local> ori, IEnumerable<Local> des)
           => $"{Url}distancematrix/json?" +
               $"origins=enc:{Encode(ori)}:&" +
               $"destinations=enc:{Encode(des)}:&" +
               $"sensor=false&key={Key}";



        string ConvNumber(double num)
            => Math.Round(num, 13).ToString().Replace(',', '.');

        protected string ParseLocal(Local local) => $"{ConvNumber(local.Latitude)},{ConvNumber(local.Longitude)}";

        protected static string Encode(IEnumerable<Local> points)
        {
            var str = new StringBuilder();

            var encodeDiff = (Action<int>)(diff =>
            {
                var shifted = diff << 1;
                if (diff < 0)
                    shifted = ~shifted;
                var rem = shifted;
                while (rem >= 0x20)
                {
                    str.Append((char)((0x20 | (rem & 0x1f)) + 63));
                    rem >>= 5;
                }
                str.Append((char)(rem + 63));
            });

            var lastLat = 0;
            var lastLng = 0;
            foreach (var point in points)
            {
                var lat = (int)Math.Round(point.Latitude * 1E5);
                var lng = (int)Math.Round(point.Longitude * 1E5);
                encodeDiff(lat - lastLat);
                encodeDiff(lng - lastLng);
                lastLat = lat;
                lastLng = lng;
            }
            return str.ToString();
        }


    }
}