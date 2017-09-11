using PathFinder.Routes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public interface IGenome
    {
        RouteMap Map { get; set; }
        List<Node> ListNodes { get; set; }
        List<Route> ListRoutes { get; set; }
        double Fitness { get; set; }
        void Save();
        bool IsEqual(IGenome genome);
        Task CalcRoutesAsync();
    }
}