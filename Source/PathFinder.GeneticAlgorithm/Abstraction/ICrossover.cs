namespace Pathfinder.Abstraction
{
    public interface ICrossover
    {
        CrossoverOperation Calc(CrossoverOperation Operation);
    }
}