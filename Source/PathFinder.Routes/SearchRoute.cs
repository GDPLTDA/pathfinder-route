using System.Linq;
using Newtonsoft.Json;
using PathFinder.Routes.GoogleMapas;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;
using System.Globalization;

namespace PathFinder.Routes
{
    public class SearchRoute
    {
        static Dictionary<string, Route> RouteCache = new Dictionary<string, Route>();
        string Url = "http://maps.googleapis.com/maps/api/";

        public Route[] GetRoutes(params string[] destinations)
        {
            var routes = new Route[destinations.Length - 1];

            for (int i = 1; i < destinations.Length; i++)
            {
                routes[i - 1] = GetRoute(destinations[i - 1], destinations[i]);
            }
            return routes;
        }

        public Route GetRoute(MapPoint origin, MapPoint destination)
        {
            return GetRoute(origin.Name, destination.Name);
        }
        public Route GetRoute(string origin, string destination)
		{
            var key = $"{origin}|{destination}";

            if (RouteCache.ContainsKey(key))
                return RouteCache[key];

            Console.WriteLine($"Buscando... {origin}->{destination}");
            var request = GetRequestRoute(origin, destination);
			var response = request.GetResponse();

            var route = new Route();

            using (var reader = new StreamReader(response.GetResponseStream()))
			{
				var data = JsonConvert.DeserializeObject<RouteRoot>(reader.ReadToEnd());

                if (data != null)
				{
                    if(!data.routes.Any())
                        throw new System.Exception($"{origin} -> {destination} error!");

                    foreach (var r in data.routes)
                    {
                        foreach (var l in r.legs)
                        {
                            route.Origin = new MapPoint(origin,l.start_location);
                            route.Destination = new MapPoint(destination, l.end_location);
                            route.Meters = l.distance.value;
                            route.Seconds = l.duration.value;
                        }
                    }
                }
			}
            RouteCache.Add(key, route);
            return route;
		}
        public void SaveRouteImage(List<Route> listRoutes)
        {
            var request = GetRequestStaticMap(listRoutes);

            var lsResponse = string.Empty;
            using (var lxResponse = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new BinaryReader(lxResponse.GetResponseStream()))
                {
                    var lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                    using (var lxFS = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\34891.jpg", FileMode.Create))
                    {
                        lxFS.Write(lnByte, 0, lnByte.Length);
                    }
                }
            }
        }
        WebRequest GetRequestRoute(string origem, string destino)
        {
		    var url = string.Format(
                "{0}directions/json?origin={1}&destination={2}&sensor=false", Url, origem, destino);

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
            var url = string.Format(
                "{0}staticmap?path={1}&markers={1}&size=512x512", Url, strbuild.ToString().Substring(1));

            return WebRequest.Create(url);
        }
        public string ConvNumber(double num)
        {
            return Math.Round(num,6).ToString().Replace(',', '.');
        }
    }
}