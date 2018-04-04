using PathFinder;
using PathFinder.Routes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouteGA.Models
{
    public class RoteiroViewModel
    {
        public LocalViewModel Origem { get; set; }

        public IList<LocalViewModel> Destinos { get; set; }

        public int NumeroEntregadores { get; set; }

        public string DhSaida { get; set; }
        public string DhLimite { get; set; }


        async internal Task<PRVJTConfig> ToPRVJTConfig(IRouteService routeService)
        {
            var dataLimite = DateTime.Parse(DhLimite);
            var dataSaida = DateTime.Parse(DhSaida);

            var config = new PRVJTConfig
            {
                Map = new Roteiro(routeService, Origem.Endereco, Origem.Endereco, dataSaida, dataLimite)
            };

            config.Map.DataSaida = dataSaida;

            config.DtLimite = dataLimite;
            config.NumEntregadores = NumeroEntregadores == 0 ? int.MaxValue : NumeroEntregadores;

            foreach (var item in Destinos)
            {
                var map = new Local(item.Endereco, item.Endereco)
                {
                    Period = new Period(item.DhInicial, item.DhFinal, item.MinutosEspera)
                };
                await config.Map.AddDestination(map);
            }

            return config;
        }
    }
}
