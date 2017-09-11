using System.Linq;
using Newtonsoft.Json;
using PathFinder.Routes.GoogleMapas;
using PathFinder;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace PathFinder.Routes
{
    public class SearchRoute
    {
        static readonly ConcurrentDictionary<string, Route> RouteCache = new ConcurrentDictionary<string, Route>();
        readonly string Url = "http://maps.googleapis.com/maps/api/";

        public async Task<Route[]> GetRoutesAsync(params string[] destinations)
        {
            var routes = new Route[destinations.Length - 1];

            for (int i = 1; i < destinations.Length; i++)
            {
                routes[i - 1] = await GetRouteAsync(destinations[i - 1], destinations[i]);
            }
            return routes;
        }

        public async Task<Route> GetRouteAsync(MapPoint origin, MapPoint destination)
        {
            return await GetRouteAsync(origin.Name, destination.Name);
        }
        public async Task<Route> GetRouteAsync(string origin, string destination)
        {
            var key = $"{origin}|{destination}";

            if (RouteCache.ContainsKey(key))
                return RouteCache[key];

            using (var c1 = new ConsoleFont(ConsoleColor.White))
            {
                Console.WriteLine($"Buscando : {origin}->{destination}");
                var request = GetRequestRoute(origin, destination);
                var ret = await ReadRequestAsync(key, request);

                using (var color = new ConsoleFont(ConsoleColor.Green))
                    Console.WriteLine($"Terminado : {origin}->{destination}");

                return ret;
            }
        }
        public async Task<Route> GetRouteAsync(double origin, double destination)
        {
            var key = $"{origin}|{destination}";

            if (RouteCache.ContainsKey(key))
                return RouteCache[key];

            Console.WriteLine($"Buscando... {origin}->{destination}");
            var request = GetRequestRoute(origin, destination);

            return await ReadRequestAsync(key, request);
        }
        public async Task<Route> ReadRequestAsync(string key, WebRequest request)
        {
            var response = request.GetResponse();

            var route = new Route();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var json = await reader.ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<RouteRoot>(json);

                if (data != null)
                {
                    foreach (var r in data.routes)
                    {
                        if (!data.routes.Any())
                            throw new System.Exception($"{key} error!");
                        foreach (var l in r.legs)
                        {
                            route.Origin = new MapPoint(l.start_address, l.start_location);
                            route.Destination = new MapPoint(l.end_address, l.end_location);
                            route.Meters = l.distance.value;
                            route.Seconds = l.duration.value;

                            if (!RouteCache.TryAdd(key, route))
                                using (var c = new ConsoleFont(ConsoleColor.Red))
                                    Console.WriteLine($"CONFLICT AT {key}");

                            var xx = GetRequestRoute(route.Origin.Latitude, route.Origin.Longitude);
                        }
                    }
                }
            }
            return route;
        }

        public void SaveRouteImage(List<Route> listRoutes)
        {
            var request = GetRequestStaticMap(listRoutes);

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
        WebRequest GetRequestRoute(string origem, string destino)
        {
            var url = $"{Url}directions/json?origin={origem}&destination={destino}&sensor=false";

            return WebRequest.Create(url);
        }
        WebRequest GetRequestRoute(double latitude, double longitude)
        {
            var url = $"{Url}geocode/json?latlng={ConvNumber(latitude)},{ConvNumber(longitude)}&sensor=false";

            return WebRequest.Create(url);
        }
        WebRequest GetRequestStaticMap(List<Route> listRoutes)
        {
            var strbuild = new StringBuilder();

            foreach (var route in listRoutes)
            {
                strbuild.Append($"|{ConvNumber(route.Origin.Latitude)},{ConvNumber(route.Origin.Longitude)}|" +
                                $"{ConvNumber(route.Destination.Latitude)},{ConvNumber(route.Destination.Longitude)}");
            }
            var url = $"{Url}staticmap?path={strbuild.ToString().Substring(1)}&markers={strbuild.ToString().Substring(1)}&size=512x512";

            return WebRequest.Create(url);
        }
        public static string ConvNumber(double num)
            => Math.Round(num,6).ToString().Replace(',', '.');
    }
}