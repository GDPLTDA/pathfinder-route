using CalcRoute.GeneticAlgorithm.Abstraction;
using System;

namespace CalcRoute.GeneticAlgorithm
{
    public sealed class RandomSingleton
    {
        private static Lazy<IRandom> Rand = new Lazy<IRandom>(new RandomAdapter());

        public static IRandom Instance => Rand.Value;
    }
}