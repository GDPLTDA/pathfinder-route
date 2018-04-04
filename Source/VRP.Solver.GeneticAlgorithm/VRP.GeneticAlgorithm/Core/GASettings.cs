namespace VRP.GeneticAlgorithm
{
    public class GASettings
    {
        public int Throttle { get; set; } = 1;
        public MutateEnum Mutation { get; set; } = MutateEnum.DM;
        public CrossoverEnum Crossover { get; set; } = CrossoverEnum.OBX;

        public double CrossoverRate { get; set; } = 0.5;
        public double MutationRate { get; set; } = 0.001;

        public int PopulationSize { get; set; } = 100;
        public int GenerationLimit { get; set; } = 20;
        public int BestSolutionToPick { get; set; } = 0;
    }
}
