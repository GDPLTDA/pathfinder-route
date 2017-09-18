using PathFinder.Routes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public interface IGenome
    {
        RouteMap Map { get; set; }
        List<Node> ListNodes { get; set; }
        List<Route> ListRoutes { get; set; }
        DateTime Finish { get; set; }

        double Fitness { get; }
        void Save();
        bool IsEqual(IGenome genome);

        void CalcFitness(IFitness fitness);

        Task CalcRoutesAsync();
    }
}