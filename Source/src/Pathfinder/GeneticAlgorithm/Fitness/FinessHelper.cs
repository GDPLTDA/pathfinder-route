using System.Collections.Generic;
namespace Pathfinder.Genetic_Algorithm.Fitness
{
    public static class FinessHelper
    {
        public static Dictionary<Xy, int> RepeatControl = new Dictionary<Xy, int>();
    }
    public struct Xy
    {
        public int x;
        public int y;
        public override string ToString()
        {
            return $"{x},{y}";
        }
    }
}
