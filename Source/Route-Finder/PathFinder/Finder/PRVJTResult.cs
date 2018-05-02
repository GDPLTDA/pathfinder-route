using PathFinder.GeneticAlgorithm;
using System.Collections.Generic;
using System.Linq;

namespace PathFinder
{
    public class PRVJTResult
    {
        public bool Erro { get; set; }
        public TipoErro TipoErro { get; set; }
        public string Messagem { get; set; }

        public PRVJTResult Register(string msg)
        {

            Erro = true;
            Messagem = msg;

            return this;
        }
        public PRVJTResult Register(TipoErro erro)
        {
            TipoErro = erro;
            Erro = true;
            Messagem = erro.GetDescription();
            return this;
        }
    }
    public class FinderResult : PRVJTResult
    {
        public List<Truck> ListEntregadores { get; set; }

        public bool Concluido => !ListEntregadores.Any(o => o.Routes.Any(r => r.DhChegada > o.DepotBack.DhChegada));

        public new FinderResult Register(string msg)
        {
            base.Register(msg);

            return this;
        }
        public new FinderResult Register(TipoErro erro)
        {
            base.Register(erro);

            return this;
        }
    }

    //public class EntregadorResult : PRVJTResult
    //{
    //    public EntregadorResult(Entregador entregador)
    //    {
    //        Entregador = entregador;
    //    }
    //    public Entregador Entregador { get; set; }

    //    public new EntregadorResult Register(string msg)
    //    {
    //        base.Register(msg);

    //        return this;
    //    }
    //    public new EntregadorResult Register(TipoErro erro)
    //    {
    //        base.Register(erro);

    //        return this;
    //    }
    //}
}
