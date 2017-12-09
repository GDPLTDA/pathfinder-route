using System;
using PathFinder.Routes;

namespace RouteGA.Models
{
    public class LocalModelView
    {
        public string Endereco { get; set; }
        public TimeSpan DhInicial { get; set; }
        public TimeSpan DhFinal { get; set; }
        public int MinutosEspera { get; set; }

        public LocalModelView(Local local)
        {
            Endereco = local.Endereco;
            DhInicial = local.Period.From;
            DhFinal = local.Period.To;
            MinutosEspera = local.Period.Descarga;
        }
    }
}
