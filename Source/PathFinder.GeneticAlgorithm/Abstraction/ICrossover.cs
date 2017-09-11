namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public interface ICrossover
    {
        CrossoverOperation Make(CrossoverOperation Operation);

        CrossoverOperation Make(IGenome mon, IGenome dad);
    }
}