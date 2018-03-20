using System;
using System.Collections.Generic;
using System.Linq;

namespace VRP.GeneticAlgorithm
{
    public static class Extensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list) => list.Shuffle(new Random());
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list, Random r) =>
                from value in list
                orderby r.Next()
                select value;

        public static T[] ToArray<T>(this (T, T) self) => new T[] { self.Item1, self.Item2 };
    }
}
