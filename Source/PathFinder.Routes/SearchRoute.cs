using System.Linq;
using Newtonsoft.Json;
using PathFinder.Routes.GoogleMapas;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace PathFinder.Routes
{
    public class SearchRoute
    {
        Dictionary<string, Route> RouteCache = new Dictionary<string, Route>();
        string Url = "http://maps.googleapis.com/maps/api/directions/json?";

        public Route[] GetRoutes(params string[] destinations)
        {
            var routes = new Route[destinations.Length - 1];

            for (int i = 1; i < destinations.Length; i++)
            {
                routes[i - 1] = GetRoute(destinations[i - 1], destinations[i]);
            }
            return routes;
        }

        public Route GetRoute(string origin, string destination)
		{
            var route = new Route();
            route.Origin = origin;
            route.Destination = destination;

            var key = $"{origin}|{destination}";

            if (RouteCache.ContainsKey(key))
                return RouteCache[key];

			var request = GetRequest(origin, destination);
			var response = request.GetResponse();

			using (var reader = new StreamReader(response.GetResponseStream()))
			{
				var data = JsonConvert.DeserializeObject<RouteRoot>(reader.ReadToEnd());

				if (data != null)
				{
					var distanciaRetornada = data.routes.Sum(r => r.legs.Sum(l => l.distance.value));
					var duracaoRetornada = data.routes.Sum(r => r.legs.Sum(l => l.duration.value));

					if (!distanciaRetornada.Equals(0))
					{
                        route.Sucess = true;
						route.Meters = distanciaRetornada;
						route.Seconds = duracaoRetornada;
					}
				}
			}
            RouteCache.Add(key, route);
            return route;
		}

        WebRequest GetRequest(string origem, string destino){
			var url = string.Format(
				"{0}origin={1}&destination={2}&sensor=false", Url, origem, destino);

            return WebRequest.Create(url);
        }
    }
}