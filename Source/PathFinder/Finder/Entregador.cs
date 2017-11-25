using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.Routes;
using System.Collections.Generic;
using System.Linq;

namespace PathFinder
{
    public class Entregador
    {
        public int Numero { get; set; }
        public MapPoint Saida => Map.Storage;
        public List<MapPoint> Pontos => Map.Destinations;
        public Route NextRoute { get; set; }
        public RouteMap Map { get; set; }
        public IGenome Genome { get; set; }
    }
}
