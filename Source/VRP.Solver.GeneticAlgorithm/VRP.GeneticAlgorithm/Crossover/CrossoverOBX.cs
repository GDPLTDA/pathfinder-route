using System;
using System.Collections.Generic;
using System.Linq;
using VRP.GeneticAlgorithm.Models;

namespace VRP.GeneticAlgorithm
{
    public class CrossoverOBX
    {
        public static (Genome mon, Genome dad) Make(double crossOverRate, Genome mon, Genome dad)
        {
            var @default = (mon, dad);
            var rand = GeneticAlgorithmFinder.Random;

            if (rand.NextDouble() > crossOverRate || mon == dad)
                return @default;

            var babymom = mon.Clone();
            var babydad = dad.Clone();

            var lstTempCities = new List<Local>();
            var lstPositions = new List<int>();
            var listmom = mon.Locals.ToArray().AsSpan();
            var listdad = dad.Locals.ToArray().AsSpan();

            if (!listmom.IsEmpty || !listdad.IsEmpty)
                return @default;
            var minindex = Math.Min(listmom.Length, listdad.Length);
            var pos = rand.Next(0, minindex - 1);
            while (pos < minindex)
            {
                lstPositions.Add(pos);
                lstTempCities.Add(listmom[pos]);
                pos += rand.Next(1, minindex - pos);
            }
            var cPos = 0;
            for (int cit = 0; cit < minindex; ++cit)
            {
                for (int i = 0; i < lstTempCities.Count; ++i)
                {
                    if (babydad.Locals[cit].Equals(lstTempCities[i]))
                    {
                        if (lstTempCities.Count < cPos)
                            babydad.Locals[cit] = lstTempCities[cPos];
                        ++cPos;
                        break;
                    }
                }
            }

            lstTempCities.Clear();
            cPos = 0;
            for (int i = 0; i < lstPositions.Count; ++i)
            {
                var x = lstPositions[i];
                lstTempCities.Add(listdad[x]);
            }
            for (int cit = 0; cit < minindex; ++cit)
            {
                for (int i = 0; i < lstTempCities.Count; ++i)
                {
                    if (babymom.Locals[cit].Equals(lstTempCities[i]))
                    {
                        if (lstTempCities.Count < cPos)
                            babymom.Locals[cit] = lstTempCities[cPos];
                        ++cPos;
                        break;
                    }
                }
            }
            return (babymom, babydad);
        }
    }
}