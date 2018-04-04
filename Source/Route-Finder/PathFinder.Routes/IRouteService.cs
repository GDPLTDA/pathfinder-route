using System.Threading.Tasks;

namespace PathFinder.Routes
{
    public interface IRouteService
    {
        Task<Local> GetPointAsync(Local local);
        Task<Rota> GetRouteAsync(Local origin, Local destination);
    }
}