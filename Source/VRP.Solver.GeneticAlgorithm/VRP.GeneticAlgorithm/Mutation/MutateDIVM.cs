using System.Collections.Generic;
using VRP.GeneticAlgorithm.Models;

namespace VRP.GeneticAlgorithm
{
    public class MutateDIVM
    {
        public static Genome Apply(double mutationRate, Genome baby)
        {
            var rand = GeneticAlgorithmFinder.Random;
            if (rand.NextDouble() > mutationRate || baby.Locals.Count < 2)
                return baby;

            var listcount = baby.Locals.Count;
            int beg, end;
            beg = end = 0;
            var spanSize = rand.Next(0, listcount);
            beg = rand.Next(0, listcount - spanSize);
            end = beg + spanSize;
            var lstTemp = new List<Local>();
            for (int i = beg; i < end; i++)
            {
                lstTemp.Add(baby.Locals[beg]);
                baby.Locals.RemoveAt(beg);
            }
            var numberOfSwaprsRequired = lstTemp.Count;
            while (numberOfSwaprsRequired != 0)
            {
                var no1 = rand.Next(0, lstTemp.Count);
                var no2 = rand.Next(0, lstTemp.Count);
                var temp = lstTemp[no1];
                lstTemp[no1] = lstTemp[no2];
                lstTemp[no2] = temp;
                --numberOfSwaprsRequired;
            }
            var insertLocation = rand.Next(0, baby.Locals.Count);
            var count = 0;
            for (int i = insertLocation; count < lstTemp.Count; i++)
            {
                baby.Locals.Insert(i, lstTemp[count]);
                count++;
            }
            return baby;
        }
    }
}