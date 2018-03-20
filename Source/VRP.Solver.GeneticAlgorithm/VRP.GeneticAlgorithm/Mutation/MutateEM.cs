namespace VRP.GeneticAlgorithm
{
    public class MutateEM
    {
        public static Genome Apply(double mutationRate, Genome baby)
        {
            var rand = GeneticAlgorithmFinder.Random;
            if (rand.NextDouble() > mutationRate || baby.Locals.Count < 2)
                return baby;

            var listcount = baby.Locals.Count;
            // Ignora o inicial
            var pos1 = rand.Next(0, listcount);
            var pos2 = pos1;
            while (pos1 == pos2)
                pos2 = rand.Next(0, listcount);
            var temp = baby.Locals[pos1];
            baby.Locals[pos1] = baby.Locals[pos2];
            baby.Locals[pos2] = temp;
            return baby;
        }
    }
}