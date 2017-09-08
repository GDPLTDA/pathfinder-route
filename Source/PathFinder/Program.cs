using PathFinder.GeneticAlgorithm;
using PathFinder.Routes;
using System;
using System.IO;
using System.Text;

namespace PathFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = new RouteMap("São Paulo SP");

            var capitais = File.ReadAllText("Capitais.txt", Encoding.GetEncoding("iso-8859-1")).Split("\r\n");

            foreach (var item in capitais)
                map.AddDestination(item);

            var finder = new GeneticAlgorithmFinder();
            var best = finder.FindPath(map);
            best.Save(); // Save uma imagem com a rota na pasta dos binarios

            foreach (var item in best.ListRoutes)
                Console.WriteLine($"{item.Origin.Name}->{item.Destination.Name}");

            Console.ReadKey();
        }
    }
}
