using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PathFinder;
using PathFinder.Routes;

namespace RouteGA.Models
{
    public class RoteiroViewModel
    {
        public LocalModelView Origem { get; set; }

        public IList<LocalModelView> Destinos { get; set; }

        public int NumeroEntregadores { get; set; }

        public DateTime DhSaida { get; set; }
        public DateTime DhLimite { get; set; }

        public RoteiroViewModel()
        {
            
        }

        async internal Task<PRVJTConfig> ToConfig()
        {
            var config = new PRVJTConfig();

            config.Map = new Roteiro(Origem.Endereco, Origem.Endereco, DhSaida, DhLimite);
            config.Map.DataSaida = DhSaida;

            config.DtLimite = DhLimite;
            config.NumEntregadores = NumeroEntregadores;

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
