using System;
using System.Collections.Generic;
using System.Linq;

namespace PathFinder.Routes
{
    public class RouteMap
    {
        public MapPoint Storage { get; set; }

        public DateTime DataSaida => Storage.Date;
        public List<MapPoint> Destinations { get; set; } = new List<MapPoint>();

        public RouteMap(string name, DateTime? now = null)
        {
            Load(name, now);
        }
        public RouteMap(MapPoint storage, DateTime? now = null)
        {
            Load(storage, now);
        }
        async void Load(string name, DateTime? now)
        {
            Load(new MapPoint(name), now);
        }
        async void Load(MapPoint point, DateTime? now)
        {
            Storage = await SearchRoute.GetPointAsync(point);

            if (now == null)
                now = DateTime.Now;

            Storage.Date = (DateTime)now;
        }

        public void AddDestinations(params string[] param)
        {
            foreach (var item in param)
                AddDestination(item);
        }
        public async void AddDestination(string destination)
        {
            var point = await SearchRoute.GetPointAsync(new MapPoint(destination));
            Destinations.Add(point);
        }
        public async void AddDestination(MapPoint mappoint)
        {
            var point = await SearchRoute.GetPointAsync(mappoint);
            Destinations.Add(point);
        }
        /// <summary>
        /// Remove o primeiro ponto, coloca o segundo ponto como primeiro
        /// </summary>
        /// <param name="list"></param>
        public void Next(IList<Route> list)
        {
            // O primeiro destino das rotas
            Storage = list.First().Destination;
            Storage.Date = DateTime.Now; // Atualiza a data inicial
            //Remove o primeiro da lista
            list.RemoveAt(0);
            //Limpa a lista de destinos
            Destinations.Clear();
            // Adiciona os destinos removendo o ponto inicial
            foreach (var item in list)
                Destinations.Add(item.Destination);
        }
    }
}
