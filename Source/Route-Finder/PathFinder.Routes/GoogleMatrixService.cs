using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace CalcRoute.Routes
{
    public class GoogleMatrixService : CachedGoogleDirectionsService, IRouteService
    {

        public GoogleMatrixService(HttpClient httpClient) : base(httpClient) { }

        Dictionary<string, Dictionary<string, Rota>> _routes;

        public override async Task Prepare(IEnumerable<Local> locals)
        {

            if (UseCache && _routes != null && _routes.Any())
                return;

            _routes = new Dictionary<string, Dictionary<string, Rota>>();
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
                        if (_routes.ContainsKey(key))
                        {
                            var dict = _routes[key];
                            foreach (var r in destinos)
                                dict.Add(r.Key, r.Value);
                        }
                        else
                            _routes.Add(key, destinos);
                    }

                }
        }


        public override Task<Rota> GetRouteAsync(Local origin, Local destination)
        {
            if (_routes == null || _routes.Count == 0)
                throw new Exception("No initialized data");

            try
            {
                var rota = _routes[ParseLocal(origin)][ParseLocal(destination)];

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

        public override void SaveCache()
        {
            if (!UseCache)
                return;

            var jsonMatrix = JsonConvert.SerializeObject(_routes);

            File.WriteAllText($"MatrixCache", jsonMatrix);


            base.SaveCache();
        }

        public override void LoadCache()
        {
            if (!UseCache)
                return;

            var routeFile = $"MatrixCache.txt";
            if (File.Exists(routeFile))
            {
                var jsonRoutes = File.ReadAllText(routeFile);
                _routes = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Rota>>>(jsonRoutes);
            }
            base.LoadCache();
        }

        string GetRequestMatrixUrl(IEnumerable<Local> ori, IEnumerable<Local> des)
           => $"{Url}distancematrix/json?" +
               $"origins=enc:{Encode(ori)}:&" +
               $"destinations=enc:{Encode(des)}:&" +
               $"sensor=false&key={Key}";

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