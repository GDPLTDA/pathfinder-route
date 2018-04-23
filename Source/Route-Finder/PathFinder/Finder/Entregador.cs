using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.Routes;
using System.Collections.Generic;
using System.Linq;

namespace PathFinder
{
    public class Entregador
    {
        public int Numero { get; set; }
        public Local Saida => Map?.Depot;
        public List<Local> Pontos => Map?.Destinations;
        public Rota NextRoute { get; set; }
        public Roteiro Map { get; set; }
        public IGenome Genome { get; set; }

        public IEnumerable<Rota> Routes => Genome.Locals;
    }
}
