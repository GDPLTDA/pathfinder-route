﻿namespace CalcRoute.GeneticAlgorithm.Abstraction
{
    public interface IMutate
    {
        double MutationRate { get; set; }
        Genome Apply(Genome baby);
    }
}