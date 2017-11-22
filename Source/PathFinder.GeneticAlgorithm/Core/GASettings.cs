using System;
using System.Collections.Generic;
using System.Text;

namespace PathFinder.GeneticAlgorithm
{
    public static class GASettings
    {
        public static double CrossoverRate { get; set; } = 0.5;
        public static double MutationRate { get; set; } = 0.001;

        public static int PopulationSize { get; set; } = 1000;
        public static int GenerationLimit { get; set; } = 100;
        public static int BestSolutionToPick { get; set; } = 5;
    }
}
