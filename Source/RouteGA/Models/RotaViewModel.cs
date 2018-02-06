using System;
namespace RouteGA.Models
{
    public class RotaViewModel
    {
        public LocalViewModel Saida { get; set; }
        public DateTime DhSaida { get; set; }
        public double Metros { get; set; }
        public double Minutos { get; set; }
        public DateTime DhChegada { get; set; }
        public LocalViewModel Chegada { get; set; }

        public RotaViewModel()
        {
        }
    }
}
