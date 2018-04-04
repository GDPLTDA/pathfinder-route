using PathFinder.GeneticAlgorithm.Factories;
using PathFinder.GeneticAlgorithm.Abstraction;
using System.Collections.Generic;
using PathFinder.Routes;

namespace PathFinder.GeneticAlgorithm
{
    public class MutateDM : AbstractMutate
    {
        public override IGenome Apply(IGenome baby)
        {
            var rand = RandomFactory.Rand;
            if (rand.NextDouble() > MutationRate || baby.ListPoints.Count < 2)
                return baby;
            var listcount = baby.ListPoints.Count;

            int beg, end;
            beg = end = 0;
            var spanSize = rand.Next(0, listcount);
            beg = rand.Next(0, listcount - spanSize);
            end = beg + spanSize;
            var lstTemp = new List<Local>();
            for (int i = beg; i < end; i++)
            {
                lstTemp.Add(baby.ListPoints[beg]);
                baby.ListPoints.RemoveAt(beg);
            }
            var insertLocation = rand.Next(0, baby.ListPoints.Count);
            var count = 0;
            for (int i = insertLocation; count < lstTemp.Count; i++)
            {
                baby.ListPoints.Insert(i, lstTemp[count]);
                count++;
            }
            return baby;
        }
    }
}