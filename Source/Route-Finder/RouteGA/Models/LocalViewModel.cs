using PathFinder.Routes;
using System;

namespace RouteGA.Models
{
    public class LocalViewModel
    {
        public string Endereco { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public string DhInicial { get; set; }
        public string DhFinal { get; set; }
        public int MinutosEspera { get; set; }

        public LocalViewModel(Local local)
        {
            if (local != null)
            {
                Endereco = local.Endereco;
                DhInicial = new DateTime(local.Period.From.Ticks).ToString("HH:mm");
                DhFinal = new DateTime(local.Period.To.Ticks).ToString("HH:mm");
                MinutosEspera = local.Period.Descarga;
                Lat = local.Latitude;
                Lng = local.Longitude;

            }
        }
    }
}
