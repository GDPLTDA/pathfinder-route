using PathFinder.GeneticAlgorithm.Abstraction;

namespace PathFinder.GeneticAlgorithm.Mutation
{
    public class SwapMutattion : AbstractMutate
    {
        private readonly IRandom random;

        public SwapMutattion(GASettings settings, IRandom random) : base(settings)
        {
            this.random = random;
        }

        public override Genome Apply(Genome baby)
        {

            if (random.NextDouble() > MutationRate)
                return baby;



            return baby;
        }
    }
}
