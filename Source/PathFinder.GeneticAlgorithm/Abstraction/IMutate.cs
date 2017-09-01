namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public interface IMutate
    {
        double MutationRate { get; set; }
        IGenome Calc(IGenome baby);
    }
}