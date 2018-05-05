using CalcRoute.GeneticAlgorithm;
using CalcRoute.Routes;
using System;

namespace CalcRoute
{
    public class PRVJTConfig
    {
        public Roteiro Map { get; set; }
        public DateTime DtLimite { get; set; } = DateTime.Now.AddDays(4);

        public GASettings Settings { get; set; }
    }
}
