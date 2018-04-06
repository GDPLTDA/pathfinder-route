using PathFinder.Routes;
using System;

namespace RouteGA.Models
{
    public class LocalViewModel
    {
        public string Endereco { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public TimeSpan DhInicial { get; set; }
        public TimeSpan DhFinal { get; set; }
        public int MinutosEspera { get; set; }

        public LocalViewModel(Local local)
        {
            if (local != null)
            {
                Endereco = local.Endereco;
                DhInicial = local.Period.From;
                DhFinal = local.Period.To;
                MinutosEspera = local.Period.Descarga;
                Lat = local.Latitude;
                Lng = local.Longitude;

            }
        }
    }
}
