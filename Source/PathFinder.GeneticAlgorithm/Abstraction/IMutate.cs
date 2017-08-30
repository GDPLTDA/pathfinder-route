namespace Pathfinder.Abstraction
{
    public interface IMutate
    {
        double MutationRate { get; set; }
        IGenome Calc(IGenome baby);
    }
}