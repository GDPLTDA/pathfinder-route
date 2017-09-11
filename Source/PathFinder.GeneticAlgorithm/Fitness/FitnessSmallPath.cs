﻿using PathFinder.GeneticAlgorithm.Abstraction;
using System.Linq;

namespace PathFinder.GeneticAlgorithm
{
    public class FitnessSmallPath : IFitness
    {
        public FitnessSmallPath()
        {

        }

        public double Calc(IGenome genome)
        {
            return genome.ListRoutes.Sum(o=>o.Meters);
        }
    }
}