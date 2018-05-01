using PathFinder.GeneticAlgorithm;
using PathFinder.Routes;
using System;

namespace PathFinder
{
    public class PRVJTConfig
    {
        public Roteiro Map { get; set; }
        public DateTime DtLimite { get; set; } = DateTime.Now.AddDays(4);

        public GASettings Settings { get; set; }
    }
}
