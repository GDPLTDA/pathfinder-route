﻿namespace CalcRoute.GeneticAlgorithm
{
    public class GASettings
    {
        public int Throttle { get; set; } = 1;  // quantidade de requests simultaneos
        public MutateEnum Mutation { get; set; } = MutateEnum.Swap;
        public CrossoverEnum Crossover { get; set; } = CrossoverEnum.SubRouteInsertion;

        public double CrossoverRate { get; set; } = 0.7;
        public double MutationRate { get; set; } = 0.05;

        public int PopulationSize { get; set; } = 200;
        public int GenerationLimit { get; set; } = 1000;
        public int BestSolutionToPick { get; set; } = 2;
        public int NumberOfTrucks { get; set; } = 1;
        public double ArriveBeforePenalty { get; set; } = 1;
        public double ArriveAfterPenalty { get; set; } = 50000;
    }
}