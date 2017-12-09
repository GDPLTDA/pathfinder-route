using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PathFinder;
using PathFinder.Routes;
using RouteGA.Models;

namespace RouteGA.Controllers
{
    [Route("api/[controller]")]
    public class RouteController : Controller
    {
        //// GET api/local
        [HttpGet("local")]
        public async Task<Local> GetLocal(string name, string endereco, string abertura, string fechamento, int espera)
        {
            var local = new Local(name, endereco);

            return await local.UpdateLocal(abertura, fechamento, espera);
        }

        // POST api/route
        [HttpPost("roteiro")]
        public async Task<IEnumerable<EntregadorModelView>> PostRoteiro([FromBody]RoteiroViewModel roteiro)
        {
            var config = await roteiro.ToConfig();
            var finder = new PRVJTFinder(config);

            var result = await finder.Run();

            if (result.Erro)
                return new EntregadorModelView[] { new EntregadorModelView() };

            return result.ListEntregadores.Select(o=> new EntregadorModelView(o));
        }
    }
}
