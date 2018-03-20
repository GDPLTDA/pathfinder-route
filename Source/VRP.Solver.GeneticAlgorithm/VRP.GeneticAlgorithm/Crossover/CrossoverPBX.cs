using System;
using System.Collections.Generic;
using System.Linq;

namespace VRP.GeneticAlgorithm
{
    public class CrossoverPBX
    {
        public static (Genome mon, Genome dad) Make(double crossOverRate, Genome mon, Genome dad)
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