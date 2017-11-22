using System.Collections.Generic;
using System.Linq;

namespace PathFinder
{
    public class PRVJTResult
    {
        public bool Erro { get; set; }
        public string Messagem { get; set; }

        public PRVJTResult Register(string msg)
        {
            Erro = true;
            Messagem = msg;

            return this;
        }
        public PRVJTResult Register(TipoErro erro)
        {
            Erro = true;
            Messagem = erro.GetDescricao();

            return this;
        }
    }
    public class FinderResult : PRVJTResult
    {
        public List<Entregador> ListEntregadores { get; set; } = new List<Entregador>();

        public bool Concluido => !ListEntregadores.Exists(o => o.Pontos.Any());
        
        public string JpgMap { get; set; }

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
    public class EntregadorResult : PRVJTResult
    {
        public EntregadorResult(Entregador entregador)
        {
            Entregador = entregador;
        }
        public Entregador Entregador { get; set; }

        public new EntregadorResult Register(string msg)
        {
            base.Register(msg);

            return this;
        }
        public new EntregadorResult Register(TipoErro erro)
        {
            base.Register(erro);

            return this;
        }
    }
}
