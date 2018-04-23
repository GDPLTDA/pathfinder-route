namespace PathFinder.GeneticAlgorithm
{
    public class GASettings
    {

        public int Throttle { get; set; } = 1;  // quantidade de requests simultaneos
        public MutateEnum Mutation { get; set; } = MutateEnum.DM;
        public CrossoverEnum Crossover { get; set; } = CrossoverEnum.OBX;

        public double CrossoverRate { get; set; } = 0.5;
        public double MutationRate { get; set; } = 0.001;

        public int NumberOfTrucks { get; set; } = 1;
        public int PopulationSize { get; set; } = 100;
        public int GenerationLimit { get; set; } = 20;
        public int BestSolutionToPick { get; set; } = 1;
        public double ArriveBeforePenalty { get; set; } = 1;
        public double ArriveAfterPenalty { get; set; } = 5;
    }
}
