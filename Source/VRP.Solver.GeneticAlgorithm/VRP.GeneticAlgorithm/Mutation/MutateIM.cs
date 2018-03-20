namespace VRP.GeneticAlgorithm
{
    public class MutateIM
    {
        public static Genome Apply(double mutationRate, Genome baby)
        {
            var rand = GeneticAlgorithmFinder.Random;
            if (rand.NextDouble() > mutationRate || baby.Locals.Count < 2)
                return baby;
            var listcount = baby.Locals.Count;
            var randomPoint = rand.Next(0, listcount);
            var tempNumber = baby.Locals[randomPoint];
            baby.Locals.RemoveAt(randomPoint);
            var insertAt = rand.Next(0, listcount);
            baby.Locals.Insert(insertAt, tempNumber);
            return baby;
        }
    }
}