using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalcRoute.Routes
{
    public interface IRouteService
    {
        Task<Local> GetPointAsync(Local local);
        Task<Rota> GetRouteAsync(Local origin, Local destination);

        Task Prepare(IEnumerable<Local> locals);

        void SaveCache(string name);

        void LoadCache();

        string GetRouteCache();

    }
}