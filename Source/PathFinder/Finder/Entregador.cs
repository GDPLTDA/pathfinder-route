using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.Routes;
using System.Collections.Generic;
using System.Linq;

namespace PathFinder
{
    public class Entregador
    {
        public int Numero { get; set; }
        public MapPoint Saida { get; set; }
        public List<MapPoint> Pontos { get; set; }
        public Route NextRoute { get; set; }
        public RouteMap Map { get; set; }
        IGenome _Genome;
        public IGenome Genome { get { return _Genome; } set { _Genome = value; AddGenome(value); } }

        public void AddGenome(IGenome genome)
        {
            if(genome.ListRoutes.Any())
                genome.ListRoutes.RemoveAt(0);

            if (genome.ListNodes.Any())
                genome.ListNodes.RemoveAt(0);
        }
    }
}
