using Microsoft.AspNetCore.Mvc;
using PathFinder;
using PathFinder.Routes;
using RouteGA.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RouteGA.Controllers
{
    [Route("api/[controller]")]
    public class RouteController : Controller
    {
        public async Task<IActionResult> Get(string name, string endereco, string abertura, string fechamento, int espera)
        {
            if (new[] { name, endereco, abertura, fechamento }.Any(e => string.IsNullOrWhiteSpace(e)))
                return BadRequest("Parâmetros inválidos");

            var local = new Local(name, endereco);

            return Ok(await local.UpdateLocal(abertura, fechamento, espera));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RoteiroViewModel roteiro)
        {
            var config = await roteiro.ToPRVJTConfig();
            var finder = new PRVJTFinder(config);


            var result = await finder.Run();

            if (result.Erro)
                return Ok(EntregadorViewModel.Empty);

            var viewmodel = result.ListEntregadores.Select(o => new EntregadorViewModel(o)).ToList();


            return Ok(viewmodel);
        }
    }
}
