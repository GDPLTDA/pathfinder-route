using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace CalcRoute.Routes
{
    public class GoogleMatrixService : CachedGoogleDirectionsService, IRouteService
    {

        public GoogleMatrixService(HttpClient httpClient) : base(httpClient) { }

        Dictionary<string, Dictionary<string, Func<Rota>>> routes;

        public override async Task Prepare(IEnumerable<Local> locals)
        {
            routes = new Dictionary<string, Dictionary<string, Func<Rota>>>();
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
                        var destinos = new Dictionary<string, Func<Rota>>();
                        for (int j = 0; j < cols.Count; j++)
                        {
                            var col = cols[j];
                            double metros = col.distance.value;
                            double segundos = col.duration.value;

                            var rotaFactory = Unclocure(bufferOrigin[i], bufferDest[j], metros, segundos);

                            destinos.Add(ParseLocal(bufferDest[j]), rotaFactory);

                        }

                        var key = ParseLocal(bufferOrigin[i]);
                        if (routes.ContainsKey(key))
                        {
                            var dict = routes[key];
                            foreach (var r in destinos)
                                dict.Add(r.Key, r.Value);
                        }
                        else
                            routes.Add(key, destinos);
                    }

                }
        }

        static Func<Rota> Unclocure(Local origem, Local destino, double metros, double segundos) =>
            () => new Rota
            {
                Origem = origem,
                Destino = destino,
                Metros = metros,
                Segundos = segundos,
            };

        public override Task<Rota> GetRouteAsync(Local origin, Local destination)
        {
            if (routes == null || routes.Count == 0)
                throw new Exception("No initialized data");

            try
            {
                var rota = routes[ParseLocal(origin)][ParseLocal(destination)]();

                return Task.FromResult(rota);
            }
            catch (Exception)
            {

                throw;
            }

        }


        string GetRequestMatrixUrl(IEnumerable<Local> ori, IEnumerable<Local> des)
           => $"{Url}distancematrix/json?" +
               $"origins=enc:{Encode(ori)}:&" +
               $"destinations=enc:{Encode(des)}:&" +
               $"sensor=false&key={Key}";

        //$"origins={string.Join("|", ori.Select(ParseLocal))}&" +
        //$"destinations={string.Join("|", des.Select(ParseLocal))}&" +
        //$"sensor=false&key={Key}";

        static string Encode(IEnumerable<Local> points)
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