using System;
namespace RouteGA.Models
{
    public class RotaViewModel
    {
        public LocalViewModel Saida { get; set; }
        public string DhSaida { get; set; }
        public string Metros { get; set; }
        public string Km { get; set; }
        public string Minutos { get; set; }
        public string DhChegada { get; set; }
        public LocalViewModel Chegada { get; set; }

        public RotaViewModel()
        {
        }
    }
}
