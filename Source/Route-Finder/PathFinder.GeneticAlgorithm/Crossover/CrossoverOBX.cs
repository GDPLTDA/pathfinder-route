using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.GeneticAlgorithm.Factories;
using PathFinder.Routes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PathFinder.GeneticAlgorithm.Crossover
{
    public class CrossoverOBX : AbstractCrossover
    {
        public CrossoverOBX(GASettings settings) : base(settings)
        {
        }

        public override CrossoverOperation Make(CrossoverOperation Operation)
        {
            var rand = RandomFactory.Rand;
            if (rand.NextDouble() > CrossoverRate || Operation.IsEqual())
                return Operation;
            var babymom = CrossoverOperation.Copy(Operation.Mom);
            var babydad = CrossoverOperation.Copy(Operation.Dad);
            var lstTempCities = new List<Local>();
            var lstPositions = new List<int>();
            var listmom = Operation.Mom.Locals;
            var listdad = Operation.Dad.Locals;

            if (!listmom.Any() || !listdad.Any())
                return Operation;

            var minindex = Math.Min(listmom.Count, listdad.Count);
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
                        if (lstTempCities.Count > cPos)
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
                        if (lstTempCities.Count > cPos)
                            babymom.Locals[cit] = lstTempCities[cPos];
                        ++cPos;
                        break;
                    }
                }
            }
            return new CrossoverOperation(babymom, babydad);
        }
    }
}