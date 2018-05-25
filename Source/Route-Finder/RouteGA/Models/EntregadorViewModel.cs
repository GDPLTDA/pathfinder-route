using CalcRoute.GeneticAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RouteGA.Models
{
    public class EntregadorViewModel
    {
        public int Id { get; set; }
        public string Mensagem { get; set; }
        public double TotalDistancia { get; set; }
        public string TotalTempo { get; set; }
        public IEnumerable<RotaViewModel> Rotas { get; private set; }
        public static EntregadorViewModel[] Empty { get => new[] { new EntregadorViewModel() }; }

        public EntregadorViewModel()
        {

        }

        public EntregadorViewModel(Truck entregador)
        {
            var rotas = entregador.Routes.Concat(new[] { entregador.DepotBack }).Select((item, id) =>
                new RotaViewModel
                {
                    Saida = new LocalViewModel(item.Origem),
                    Chegada = new LocalViewModel(item.Destino),
                    DhSaida = item.DhSaida.ToString("HH:mm:ss"),
                    DhChegada = item.DhChegada.ToString("HH:mm:ss"),
                    Metros = item.Metros.ToString(),
                    Km = item.Km.ToString("n3"),
                    Espera = ConvMinutos(item.Espera),
                    Descarga = ConvMinutos(item.Descarga),
                    Minutos = ConvMinutos(item.Segundos)
                });

            Id = entregador.Id;
            Rotas = rotas;
            TotalDistancia = Math.Round(entregador.Routes.Sum(r => r.Km), 3);
            TotalTempo = (entregador.DepotBack.DhChegada - entregador.Routes.First().DhSaida ).ToString();
        }

        string ConvMinutos(double segundos)
        {
            var span = new TimeSpan(0, 0, Convert.ToInt32(segundos));

            return $"{span.Hours.ToString().PadLeft(2, '0')}:{span.Minutes.ToString().PadLeft(2, '0')}:{span.Seconds.ToString().PadLeft(2, '0')}";
        }
    }
}
