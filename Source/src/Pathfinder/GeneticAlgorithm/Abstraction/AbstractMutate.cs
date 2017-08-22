namespace Pathfinder.Abstraction
{
    public abstract class AbstractMutate : IMutate
    {
        protected AbstractMutate()
        {
            MutationRate = Constants.MUTATION_RATE;
        }
        public double MutationRate { get; set; }
        public abstract IGenome Calc(IGenome baby);
    }
}