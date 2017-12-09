using System;
namespace RouteGA.Models
{
    public class RotaModelView
    {
        public LocalModelView Saida { get; set; }
        public DateTime DhSaida { get; set; }
        public double Metros { get; set; }
        public double Minutos { get; set; }
        public DateTime DhChegada { get; set; }
        public LocalModelView Chegada { get; set; }

        public RotaModelView()
        {
        }
    }
}
