namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public interface ICrossover
    {
        Genome[] Make(Genome mon, Genome dad);
    }
}