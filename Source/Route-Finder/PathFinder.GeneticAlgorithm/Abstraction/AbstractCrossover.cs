namespace CalcRoute.GeneticAlgorithm.Abstraction
{
    public abstract class AbstractCrossover : ICrossover
    {
        protected double CrossoverRate { get; private set; }
        protected AbstractCrossover(GASettings settings) => CrossoverRate = settings.CrossoverRate;

        public abstract Genome[] Make(Genome mon, Genome dad);
    }
}