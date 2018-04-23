namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public interface IFitness
    {
        double Calc(IGenome genome, GASettings settings);
    }
}