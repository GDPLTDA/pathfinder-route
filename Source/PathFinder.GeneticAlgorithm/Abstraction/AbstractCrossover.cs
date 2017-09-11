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

        public void Deconstruct(out IGenome mon, out IGenome dad)
        {
            mon = Mom;
            dad = Dad;
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
        public abstract CrossoverOperation Make(CrossoverOperation Operation);

        public CrossoverOperation Make(IGenome mon, IGenome dad) =>
           Make(new CrossoverOperation(mon, dad));
    }
}