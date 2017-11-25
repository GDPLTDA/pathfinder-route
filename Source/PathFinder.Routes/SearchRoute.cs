using ColoredConsole;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder.Routes
{
    public static class SearchRoute
    {
        static bool CacheActive { get; set; } = true;
        static bool Traffic { get; set; } = true;
        static ConcurrentDictionary<string, Route> RouteCache = new ConcurrentDictionary<string, Route>();
        static ConcurrentDictionary<string, MapPoint> PointCache = new ConcurrentDictionary<string, MapPoint>();
        const string Url = "https://maps.googleapis.com/maps/api/";
        const string Key = "AIzaSyBFP8cY4DSZM_7Z9k2svtu-Ktdjhq23UNI";

        static SearchRoute()
        {
            if (CacheActive)
            {
                if (File.Exists("RouteCache.txt")) {
                    RouteCache = JsonConvert.DeserializeObject<ConcurrentDictionary<string, Route>>
                        (File.ReadAllText("RouteCache.txt"));

                    if(RouteCache == null)
                        RouteCache = new ConcurrentDictionary<string, Route>();
                }
                if (File.Exists("PointCache.txt"))
                {
                    PointCache = JsonConvert.DeserializeObject<ConcurrentDictionary<string, MapPoint>>
                        (File.ReadAllText("PointCache.txt"));

                    if (PointCache == null)
                        PointCache = new ConcurrentDictionary<string, MapPoint>();
                }
            }
        }

        public static void SaveCache()
        {
            File.WriteAllText("RouteCache.Txt", JsonConvert.SerializeObject(RouteCache));
            File.WriteAllText("PointCache.Txt", JsonConvert.SerializeObject(PointCache));
        }

        public static async Task<Route> GetRouteAsync(MapPoint origin, MapPoint destination)
        {
            var key = $"{origin.Endereco}|{destination.Endereco}";

            if (RouteCache.ContainsKey(key))
                return RouteCache[key];

            var request = GetRequestPointRoute(origin, destination);
            var ret = await ReadRequestRouteAsync(origin, destination, key, request);

            ColorConsole.WriteLine($"Rota Encontrada: : {origin.Endereco} {destination.Endereco}".White());

            return ret;
        }
        public static async Task<Route> ReadRequestRouteAsync(MapPoint origin, MapPoint destination, string key, WebRequest request)
        {
            var response = request.GetResponse();

            var route = new Route();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var json = await reader.ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(json);

                if (data != null)
                {
                    var routes = data.routes as IEnumerable<dynamic>;
                    if (!routes.Any())
                    {
                        if (data.status == "OVER_QUERY_LIMIT")
                            throw new Exception("Estourou o limite diario!");

                        ColorConsole.WriteLine($"{data.status}: {key}".Red());
                    }

                    foreach (var r in routes)
                    {
                        foreach (var l in r.legs)
                        {
                            route.Origin = origin;
                            route.Destination = destination;
                            route.Meters = l.distance.value;
                            route.Seconds = l.duration.value;

                            if (CacheActive && !RouteCache.TryAdd(key, route))
                                ColorConsole.WriteLine($"CONFLICT AT {key}".Red());
                        }
                    }
                }
            }
            return route;
        }
        public async static Task<MapPoint> GetPointAsync(MapPoint mappoint)
        {
            if (PointCache.ContainsKey(mappoint.Endereco))
            {
                var retorno = PointCache[mappoint.Endereco];
                retorno.Period = mappoint.Period;
                return retorno;
            }

            var request = GetRequestAddress(mappoint.Endereco);
            var ret = await ReadRequestPointAsync(mappoint, request);
            ret.Period = mappoint.Period;
            ColorConsole.WriteLine($"Endereço Encontrado: {mappoint.Endereco} ({ret.Latitude},{ret.Longitude})".Green());

            return ret;
        }
        public async static Task<MapPoint> ReadRequestPointAsync(MapPoint mappoint, WebRequest request)
        {
            var response = request.GetResponse();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var json = await reader.ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(json);

                if (data != null)
                {
                    var results = data.results as IEnumerable<dynamic>;

                    if (!results.Any())
                        ColorConsole.WriteLine($"{data.status}: {mappoint.Endereco}".Red());

                    foreach (var r in results)
                    {
                        //mappoint.Name = r.formatted_address;
                        mappoint.Latitude = r.geometry.location.lat;
                        mappoint.Longitude = r.geometry.location.lng;

                        if (!PointCache.TryAdd(mappoint.Endereco, mappoint))
                            ColorConsole.WriteLine($"CONFLICT AT {mappoint.Endereco}".Red());
                    }
                }
            }
            return mappoint;
        }

        public static void SaveRouteImage(List<Route> listRoutes)
        {
            var request = GetRequestStaticMapRoute(listRoutes);

            var lsResponse = string.Empty;

            var arq = AppDomain.CurrentDomain.BaseDirectory + "\\MAPA.jpg";

            using (var lxResponse = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new BinaryReader(lxResponse.GetResponseStream()))
                {
                    var lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                    using (var lxFS = new FileStream(arq, FileMode.Create))
                    {
                        lxFS.Write(lnByte, 0, lnByte.Length);
                    }
                }
            }
            var psi = new ProcessStartInfo(arq)
            {
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        /// <summary>
        /// Busca o tempo,distancia entre dois pontos usando o endereço
        /// </summary>
        /// <param name="ori"></param>
        /// <param name="des"></param>
        /// <returns></returns>
        static WebRequest GetRequestNameRoute(MapPoint ori, MapPoint des)
            => WebRequest.Create(
                $"{Url}directions/json?origin={ori.Endereco}&destination={des.Endereco}&sensor=false&key={Key}");
        /// <summary>
        /// Busca o tempo,distancia entre dois pontos usando o Latitude e Longitude
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        static WebRequest GetRequestPointRoute(MapPoint ori, MapPoint des)
            => WebRequest.Create(
                $"{Url}directions/json?" +
                $"origin={ConvNumber(ori.Latitude)},{ConvNumber(ori.Longitude)}&" +
                $"destination={ConvNumber(des.Latitude)},{ConvNumber(des.Longitude)}&" +
                $"sensor=false&key={Key}");
        /// <summary>
        /// Busca os dados do ponto usando o endereço
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        static WebRequest GetRequestAddress(string address)
            => WebRequest.Create(
                $"{Url}geocode/json?address={address}&sensor=false&key={Key}");
        /// <summary>
        /// Busca uma imagem da rota total
        /// </summary>
        /// <param name="listRoutes"></param>
        /// <returns></returns>
        static WebRequest GetRequestStaticMapRoute(List<Route> listRoutes)
        {
            var strbuild = new StringBuilder();
            foreach (var route in listRoutes)
                strbuild.Append($"|{ConvNumber(route.Origin.Latitude)},{ConvNumber(route.Origin.Longitude)}|" +
                                $"{ConvNumber(route.Destination.Latitude)},{ConvNumber(route.Destination.Longitude)}");

            var url = $"{Url}staticmap?path={strbuild.ToString().Substring(1)}&markers={strbuild.ToString().Substring(1)}&size=512x512";

            return WebRequest.Create(url);
        }
        /// <summary>
        /// Converte para a busca em Json
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ConvNumber(double num)
            => Math.Round(num, 6).ToString().Replace(',', '.');
    }
}