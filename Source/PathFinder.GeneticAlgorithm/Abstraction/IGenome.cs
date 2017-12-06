using PathFinder.Routes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public interface IGenome
    {
        Roteiro Map { get; set; }
        List<Local> ListPoints { get; set; }
        List<Rota> ListRoutes { get; set; }

        double Fitness { get; }
        void Save();
        bool IsEqual(IGenome genome);

        void CalcFitness(IFitness fitness);

        Task CalcRoutesAsync();
    }
}