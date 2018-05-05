using CalcRoute.GeneticAlgorithm;
using System.Collections.Generic;
using System.Linq;

namespace CalcRoute
{
    public class PRVJTResult
    {
        public bool Erro => TipoErro != TipoErro.Concluido;
        public TipoErro TipoErro { get; set; }
        public string Messagem => TipoErro.GetDescription();

        public PRVJTResult Register(TipoErro erro)
        {
            TipoErro = erro;
            return this;
        }
    }
    public class FinderResult : PRVJTResult
    {
        public Genome BestGenome { get; set; }
        public List<Truck> ListEntregadores { get; set; }

        public bool Concluido => !ListEntregadores.Any(o => o.Routes.Any(r => r.DhChegada > o.DepotBack.DhChegada));

        public new FinderResult Register(TipoErro erro)
        {
            base.Register(erro);

            return this;
        }
    }
}
