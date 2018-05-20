using CalcRoute;
using CalcRoute.GeneticAlgorithm;
using CalcRoute.Routes;
using Microsoft.AspNetCore.Mvc;
using RouteGA.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RouteGA.Controllers
{
    [Route("api/[controller]")]
    public class RouteController : Controller
    {
        private readonly ICachedRouteService routeService;

        public RouteController(ICachedRouteService routeService)
        {
            this.routeService = routeService;
        }

        void EnsureLoadCaches()
        {
            if (routeService.HasCache)
                return;

            if (!Directory.Exists("Cache"))
                return;

            Directory
                .GetFiles("Cache")
                .Select(Path.GetFileNameWithoutExtension)
                .Select(f => f.Split("_").First())
                .Distinct()
                .ForEach(routeService.LoadCache);
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
            EnsureLoadCaches();

            var settings = new GASettings();

            routeService.UseCache = roteiro.UseCache;
            routeService.Traffic = roteiro.Traffic;

            var config = await roteiro.ToPRVJTConfig(routeService, settings);
            var finder = new PRVJTFinder(config, routeService);

            var result = await finder.Run();

            if (result.Erro)
                return Ok(new EntregadorViewModel { Mensagem = result.Messagem });

            var viewmodel = result.ListEntregadores.Select(o => new EntregadorViewModel(o)).ToList();

            try
            {
                if (!string.IsNullOrEmpty(roteiro.Name))
                    this.routeService.SaveCache(roteiro.Name);

            }
            catch
            {

            }

            return Ok(viewmodel);
        }
    }
}