namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public abstract class AbstractMutate : IMutate
    {
        protected AbstractMutate()
        {
            MutationRate = GASettings.MutationRate;
        }
        public double MutationRate { get; set; }
        public abstract IGenome Apply(IGenome baby);
    }
}