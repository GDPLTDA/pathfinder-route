using CalcRoute;
using CalcRoute.GeneticAlgorithm;
using CalcRoute.Routes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouteGA.Models
{
    public class RoteiroViewModel
    {
        public string Name { get; set; }
        public int Generations { get; set; }
        public int Population { get; set; }
        public MutateEnum Mutation { get; set; }
        public LocalViewModel Origem { get; set; }

        public IList<LocalViewModel> Destinos { get; set; }

        public int NumeroEntregadores { get; set; }

        public string DhSaida { get; set; }
        public string DhLimite { get; set; }


        async internal Task<PRVJTConfig> ToPRVJTConfig(IRouteService routeService, GASettings settings)
        {
            var dataLimite = DateTime.Parse(DhLimite);
            var dataSaida = DateTime.Parse(DhSaida);

            var config = new PRVJTConfig
            {
                Map = new Roteiro(routeService, Origem.Endereco, Origem.Endereco, dataSaida, dataLimite),
                Settings = settings
            };

            config.Map.DataSaida = dataSaida;
            
            config.DtLimite = dataLimite;
            config.Settings.NumberOfTrucks = NumeroEntregadores == 0 ? int.MaxValue : NumeroEntregadores;
            config.Settings.Mutation = Mutation;
            config.Settings.GenerationLimit = Generations;
            config.Settings.PopulationSize = Population;
            
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
