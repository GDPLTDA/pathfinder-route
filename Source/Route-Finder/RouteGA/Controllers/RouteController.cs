using Microsoft.AspNetCore.Mvc;
using PathFinder;
using PathFinder.GeneticAlgorithm;
using PathFinder.Routes;
using RouteGA.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RouteGA.Controllers
{
    [Route("api/[controller]")]
    public class RouteController : Controller
    {
        private readonly IRouteService routeService;

        public RouteController(IRouteService routeService)
        {
            this.routeService = routeService;
        }

        public async Task<IActionResult> Get(string name, string endereco, string abertura, string fechamento, int espera)
        {
            if (new[] { name, endereco, abertura, fechamento }.Any(e => string.IsNullOrWhiteSpace(e)))
                return BadRequest("Parâmetros inválidos");

            var local = new Local(name, endereco)
            {
                Period = new Period(abertura, fechamento, espera)
            };

            local = await routeService.GetPointAsync(local);

            return Ok(local);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RoteiroViewModel roteiro)
        {
            var settings = new GASettings();
            var config = await roteiro.ToPRVJTConfig(routeService);
            var finder = new PRVJTFinder(config, routeService, settings);


            var result = await finder.Run();

            if (result.Erro)
                return Ok(new EntregadorViewModel { Mensagem = result.Messagem });

            var viewmodel = result.ListEntregadores.Select(o => new EntregadorViewModel(o)).ToList();


            return Ok(viewmodel);
        }
    }
}
