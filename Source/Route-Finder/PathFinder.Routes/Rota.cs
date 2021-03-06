﻿using System;
namespace CalcRoute.Routes
{
    public class Rota
    {
        public int Ordem { get; set; }
        public Local Origem { get; set; }
        public Local Destino { get; set; }
        public bool Late { get; set; }

        public double Metros { get; set; }
        public double Km { get { return Metros / 1000; } }

        public double Segundos { get; set; }
        public double Minutos { get { return Segundos / 60; } }

        public DateTime DhSaida { get; set; }
        public DateTime DhChegada { get; set; }

        public double Espera { get; set; }
        public double Descarga { get; set; }

        public Rota()
        {

        }

        public bool Equals(Rota obj) => Origem.Equals(obj.Origem) && Destino.Equals(obj.Destino);

        public override string ToString() => $"({Late }){Origem.ToString()}->{Destino.ToString()}";
    }
}