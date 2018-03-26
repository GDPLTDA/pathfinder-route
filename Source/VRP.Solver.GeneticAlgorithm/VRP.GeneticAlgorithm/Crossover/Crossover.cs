using System;
using System.Collections.Generic;
using System.Linq;
using VRP.GeneticAlgorithm.Models;

namespace VRP.GeneticAlgorithm
{
    public class Crossover
    {
        public static (Genome mon, Genome dad) CrossoverOBX(double crossOverRate, Genome mon, Genome dad)
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


        public static (Genome mon, Genome dad) CrossoverPBX(double crossOverRate, Genome mon, Genome dad)
        {
            var @default = (mon, dad);
            var rand = GeneticAlgorithmFinder.Random;

            if (rand.NextDouble() > crossOverRate || mon == dad)
                return @default;

            var babymom = mon.Clone();
            var babydad = dad.Clone();

            var lstPositions = new List<int>();
            var listmom = mon.Locals.ToArray();
            var listdad = dad.Locals.ToArray();
            var minindex = Math.Min(listmom.Length, listdad.Length);

            var Pos = rand.Next(0, minindex - 1);
            while (Pos < minindex)
            {
                lstPositions.Add(Pos);
                Pos += rand.Next(1, minindex - Pos);
            }
            for (int pos = 0; pos < lstPositions.Count; ++pos)
            {
                babymom.Locals[lstPositions[pos]] = listmom[lstPositions[pos]];
                babydad.Locals[lstPositions[pos]] = listdad[lstPositions[pos]];
            }
            int c1, c2;
            c1 = c2 = 0;
            for (int pos = 0; pos < minindex; pos++)
            {
                while (c2 < minindex)
                    ++c2;
                if (c2 < babydad.Locals.Count)
                    if (!babydad.Locals.Any(i => i.Equals(listmom[pos])))
                        babydad.Locals[c2] = listmom[pos];
                while (c1 < minindex)
                    ++c1;
                if (c1 < babymom.Locals.Count)
                    if (!babymom.Locals.Any(i => i.Equals(listdad[pos])))
                        babymom.Locals[c1] = listdad[pos];
            }
            return (babymom, babydad);
        }

    }



}