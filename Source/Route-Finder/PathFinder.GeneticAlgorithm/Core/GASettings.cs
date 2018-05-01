namespace PathFinder.GeneticAlgorithm
{
    public class GASettings
    {

        public int Throttle { get; set; } = 1;  // quantidade de requests simultaneos
        public MutateEnum Mutation { get; set; } = MutateEnum.Swap;
        public CrossoverEnum Crossover { get; set; } = CrossoverEnum.SubRouteInsertion;

        public double CrossoverRate { get; set; } = 0.5;
        public double MutationRate { get; set; } = 0.001;

        public int NumberOfTrucks { get; set; } = 1;
        public int PopulationSize { get; set; } = 1000;
        public int GenerationLimit { get; set; } = 100;
        public int BestSolutionToPick { get; set; } = 1;
        public double ArriveBeforePenalty { get; set; } = 100;
        public double ArriveAfterPenalty { get; set; } = 500;
    }
}
