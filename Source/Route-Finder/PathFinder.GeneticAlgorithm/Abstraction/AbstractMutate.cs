namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public abstract class AbstractMutate : IMutate
    {
        protected AbstractMutate(GASettings settings)
        {
            MutationRate = settings.MutationRate;
        }
        public double MutationRate { get; set; }
        public abstract Genome Apply(Genome baby);
    }
}