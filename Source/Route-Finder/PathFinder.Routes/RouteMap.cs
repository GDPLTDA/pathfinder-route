using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PathFinder.Routes
{
    public class Roteiro
    {
        public Local MainStorage { get; set; }
        public Local Depot { get; set; }
        public readonly IRouteService _routeService;
        public DateTime DataSaida { get; set; }
        public DateTime DataVolta { get; set; }
        public List<Local> Destinations { get; set; } = new List<Local>();


        private Roteiro(IRouteService routeService) { _routeService = routeService; }

        public Roteiro(IRouteService routeService, string name, string endereco, DateTime saida, DateTime volta)
        {
            _routeService = routeService;
            DataSaida = saida;
            DataVolta = volta;
            Load(name, endereco).Wait();
        }
        public Roteiro Clone() => new Roteiro(_routeService)
        {
            Depot = Depot,
            MainStorage = MainStorage,
            DataSaida = DataSaida,
            DataVolta = DataVolta,
        };


        async Task Load(string name, string endereco) =>
            await Load(new Local(name, endereco));

        async Task Load(Local point)
        {
            Depot = await _routeService.GetPointAsync(point);
            MainStorage = Depot;
            Depot.Period = new Period(DataSaida, DataVolta, 0);
        }

        public async Task AddDestinations(params string[] param)
        {
            foreach (var item in param)
                await AddDestination(item);
        }
        public async Task AddDestination(string destination)
        {
            var point = await _routeService.GetPointAsync(new Local(destination));
            Destinations.Add(point);
        }
        public async Task AddDestination(string endereco, string abertura, string fechamento, int espera)
        {
            var point = await _routeService.GetPointAsync(
                        new Local(endereco, endereco)
                        {
                            Period = new Period(abertura, fechamento, espera)
                        }
                        );

            Destinations.Add(point);
        }
        public async Task AddDestination(Local mappoint)
        {
            var point = await _routeService.GetPointAsync(mappoint);
            Destinations.Add(point);
        }

        /// <summary>
        /// Remove o primeiro ponto, coloca o segundo ponto como primeiro
        /// </summary>
        /// <param name="list"></param>
        public void Next(IList<Rota> list)
        {
            //O primeiro destino das rotas
            Depot = list.First().Destino;
            DataSaida = list.First().DhChegada.AddMinutes(Depot.Period.Descarga);
            //Remove o primeiro da lista
            list.RemoveAt(0);
            // remove a volta ao estoque
            list.Remove(list.Last());
            //Limpa a lista de destinos
            Destinations.Clear();
            // Adiciona os destinos removendo o ponto inicial
            foreach (var item in list)
                Destinations.Add(item.Destino);
        }
    }
}
