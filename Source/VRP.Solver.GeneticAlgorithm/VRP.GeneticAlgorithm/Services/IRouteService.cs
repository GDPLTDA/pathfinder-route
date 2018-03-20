using System.Collections.Generic;
using System.Threading.Tasks;
using VRP.GeneticAlgorithm.Models;

namespace VRP.GeneticAlgorithm
{
    public interface IRouteService
    {
        Task<IReadOnlyCollection<Route>> CalcFullRoute(IEnumerable<Local> locals);
        Task<Local> GetPointAsync(string name, string adress);
        Task<Route> GetRouteAsync(Local origin, Local destination);
    }
}