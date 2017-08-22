namespace Pathfinder.Abstraction
{
    public interface IGeneticAlgorithm : IFinder
    {
        IFitness Fitness { get; set; }
        IMutate Mutate { get; set; }
        ICrossover Crossover { get; set; }
        ISelection Selection { get; set; }

        void Configure(IFitness fItness, IMutate mutate, ICrossover crossover, ISelection selection);



        int Generations { get; set; }
    }
}