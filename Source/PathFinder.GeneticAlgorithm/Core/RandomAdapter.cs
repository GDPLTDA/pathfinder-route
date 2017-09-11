using PathFinder.GeneticAlgorithm.Abstraction;
using System;
using System.Security.Cryptography;

namespace PathFinder
{
    public class RandomAdapter : IRandom
    {
        readonly Random me;
        readonly RandomNumberGenerator trueRandom;

        public  RandomAdapter()
        {
            me = new Random();
        }
        public int Next() => me.Next();
        public int Next(int maxValue) => me.Next(maxValue);
        public int Next(int minValue, int maxValue) => me.Next(minValue,maxValue);
        public void NextBytes(byte[] buffer) => me.NextBytes(buffer);
        public double NextDouble() => me.NextDouble();
    }
}