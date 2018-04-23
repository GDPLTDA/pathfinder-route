using PathFinder.GeneticAlgorithm.Core;
using PathFinder.Routes;
using System.Threading.Tasks;

namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public interface IGenome
    {
        Roteiro Map { get; set; }
        TruckCollection Locals { get; set; }

        double Fitness { get; }
        bool IsEqual(IGenome genome);

        void CalcFitness(IFitness fitness);
        GASettings Settings { get; }
        Task CalcRoutesAsync(IRouteService routeService);
    }
}