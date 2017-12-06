using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PathFinder;
using PathFinder.Routes;

namespace RouteGA.Controllers
{
    [Route("api/[controller]")]
    public class RouteController : Controller
    {
        Roteiro Roteiro { get; set; }

        // GET api/route
        [HttpGet]
        public IEnumerable<Entregador> Get()
        {
            return new Entregador[] { new Entregador()};
        }

        // GET api/route/5
        [HttpGet("{id}")]
        public Entregador Get(int entregador)
        {
            return new Entregador();
        }
        //// GET api/route/5
        [HttpGet("local")]
        public async Task<Local> GetLocal(string name, string endereco, string abertura, string fechamento, int espera)
        {
            var local = new Local(name, endereco);

            return await local.UpdateLocal(abertura, fechamento, espera);
        }

        // POST api/route
        [HttpPost("roteiro")]
        public async Task PostRoteiro([FromBody]Local local)
        {
            Roteiro = new Roteiro(local);
        }

        // POST api/route
        [HttpPost("destino")]
        public async Task PostLocal([FromBody]Local local)
        {
            await Roteiro.AddDestination(local);
        }

        //// POST api/route
        //[HttpPost]
        //public void Post([FromBody]Roteiro roteiro)
        //{
        //    Roteiro = roteiro;
        //}

        // PUT api/route/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/route/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
