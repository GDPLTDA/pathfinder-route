using System;
using System.Collections.Generic;
using System.Text;

namespace PathFinder.GeneticAlgorithm
{
    public static class GASettings
    {

        public static int Throttle { get; set; } = 1;  // quantidade de requests simultaneos
        public static MutateEnum Mutation { get; set; } = MutateEnum.DM;
        public static CrossoverEnum Crossover { get; set; } = CrossoverEnum.OBX;

        public static double CrossoverRate { get; set; } = 0.5;
        public static double MutationRate { get; set; } = 0.001;

        public static int PopulationSize { get; set; } = 100;
        public static int GenerationLimit { get; set; } = 20;
        public static int BestSolutionToPick { get; set; } = 2;
    }
}
