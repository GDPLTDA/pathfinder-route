using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PathFinder.Routes
{
    public class RouteMap
    {
        public MapPoint Storage { get; set; }

        public DateTime DataSaida { get; set; }
        public List<MapPoint> Destinations { get; set; } = new List<MapPoint>();

        public RouteMap(string name, string endereco, DateTime saida)
        {
            DataSaida = saida;
            Load(name, endereco);
        }
        public RouteMap(RouteMap map)
        {
            Storage = map.Storage;
            DataSaida = map.DataSaida;
        }
        
        async Task Load(string name, string endereco) =>
            await Load(new MapPoint(name, endereco));

        async Task Load(MapPoint point)
        {
            Storage = await SearchRoute.GetPointAsync(point);
        }

        public async Task AddDestinations(params string[] param)
        {
            foreach (var item in param)
                await AddDestination(item);
        }
        public async Task AddDestination(string destination)
        {
            var point = await SearchRoute.GetPointAsync(new MapPoint(destination));
            Destinations.Add(point);
        }
        public async Task AddDestination(MapPoint mappoint)
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
            DataSaida = list.First().DtChegada;
            //Storage.Date = now; // Atualiza a data inicial
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
