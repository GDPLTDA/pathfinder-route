using System;
using System.Collections.Generic;
using PathFinder;

namespace RouteGA.Models
{
    public class EntregadorModelView
    {
        public IEnumerable<RotaModelView> Rotas { get; set; }

        public EntregadorModelView(){
            
        }

        public EntregadorModelView(Entregador entregador)
        {
            Rotas = new List<RotaModelView>();

            foreach (var item in entregador.Routes)
            {
                var rota = new RotaModelView()
                {
                    Saida = new LocalModelView(item.Origem),
                    Chegada = new LocalModelView(item.Destino),
                    DhChegada = item.DhChegada,
                    Metros = item.Metros,
                    Minutos = item.Minutos,

                };
            }
        }
    }
}
