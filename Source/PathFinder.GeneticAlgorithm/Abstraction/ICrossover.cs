namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public interface ICrossover
    {
        CrossoverOperation Calc(CrossoverOperation Operation);
    }
}