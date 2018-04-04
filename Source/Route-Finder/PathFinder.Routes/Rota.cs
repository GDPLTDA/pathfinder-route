using System;
namespace PathFinder.Routes
{
    public class Rota
    {
        public int Ordem { get; set; }
        public Local Origem { get; set; }
        public Local Destino { get; set; }

        public double Metros { get; set; }

        public double Segundos { get; set; }
        public double Minutos { get { return Segundos / 60; } }

        public DateTime DhChegada { get; set; }

        public Rota()
        {

        }

        public bool Equals(Rota obj)
        {
            return Origem.Equals(obj.Origem) && Destino.Equals(obj.Destino);
        }
    }
}