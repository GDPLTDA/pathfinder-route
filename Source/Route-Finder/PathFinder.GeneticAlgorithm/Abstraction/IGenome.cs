using PathFinder.Routes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public interface IGenome
    {
        Roteiro Map { get; set; }
        IList<Local> Locals { get; set; }
        IList<Truck> Trucks { get; set; }

        double Fitness { get; }
        bool IsEqual(IGenome genome);

        void CalcFitness(IFitness fitness);

        Task CalcRoutesAsync(IRouteService routeService);
    }
}