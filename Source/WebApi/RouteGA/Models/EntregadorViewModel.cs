using PathFinder;
using System.Collections.Generic;
using System.Linq;

namespace RouteGA.Models
{
    public class EntregadorViewModel
    {
        public int Id { get; set; }
        public IEnumerable<RotaViewModel> Rotas { get; private set; }
        public static EntregadorViewModel[] Empty { get => new[] { new EntregadorViewModel() }; }

        public EntregadorViewModel()
        {

        }

        public EntregadorViewModel(Entregador entregador)
        {
            var rotas = entregador.Routes.Select(item =>
                new RotaViewModel
                {
                    Saida = new LocalViewModel(item.Origem),
                    Chegada = new LocalViewModel(item.Destino),
                    DhChegada = item.DhChegada,
                    Metros = item.Metros,
                    Minutos = item.Minutos,

                });

            Id = entregador.Numero;
            Rotas = rotas;
        }
    }
}
