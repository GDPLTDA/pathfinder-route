namespace CalcRoute.GeneticAlgorithm.Abstraction
{
    public interface IFitness
    {
        double Calc(Genome genome, GASettings settings);
    }
}