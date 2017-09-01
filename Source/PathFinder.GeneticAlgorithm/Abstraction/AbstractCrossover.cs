namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public class CrossoverOperation
    {
        public IGenome Mom { get; set; }
        public IGenome Dad { get; set; }
        public CrossoverOperation()
        {
        }
        public CrossoverOperation(IGenome mon, IGenome dad)
        {
            Dad = mon;
            Mom = dad;
        }

        public bool IsEqual()
        {
            return Mom.Equals(Dad);
        }
        public static IGenome Copy(IGenome genome)
        {
            return new Genome(genome);
        }
    }
    public abstract class AbstractCrossover : ICrossover
    {
        protected AbstractCrossover()
        {
            CrossoverRate = GASettings.CrossoverRate;
        }
        protected double CrossoverRate { get; set; }
        CrossoverOperation Operation { get; set; }
        public abstract CrossoverOperation Calc(CrossoverOperation Operation);
    }
}