namespace Pathfinder.Abstraction
{
    public interface IFitness
    {
        double Penalty { get; set; }
        IHeuristic Heuristic { get; set; }

        double Calc(IGenome listnode);
    }
}