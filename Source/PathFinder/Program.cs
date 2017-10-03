using PathFinder.GeneticAlgorithm;
using PathFinder.Routes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder
{
    class Program
    {
        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

        static List<Entregador> Entregadores { get; set; } = new List<Entregador>();

        static DateTime DtAgora { get; set; } = DateTime.Now;
        static DateTime DtEntrega { get; set; } = DateTime.Now.AddDays(4);

        static async Task MainAsync(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            
            using (var color = new ConsoleFont(ConsoleColor.White))
                Console.WriteLine($"Buscando Endereços...");

            var map = new RouteMap("São Paulo SP", DtAgora);

            var capitais = File.ReadAllText("Capitais.txt", Encoding.GetEncoding("iso-8859-1")).Split("\r\n");

            foreach (var item in capitais)
                map.AddDestination(item);

            sw.Stop();
            using (var color = new ConsoleFont(ConsoleColor.Red))
                Console.WriteLine($"Tempo ({sw.Elapsed.Minutes}:{sw.Elapsed.Seconds})...");

            sw.Reset();
            sw.Start();
                
            using (var color = new ConsoleFont(ConsoleColor.White))
                Console.WriteLine($"Dividindo Rotas...");

            var listroutes = new List<Route>();

            while (map.Destinations.Any())
            {
                var finder = new GeneticAlgorithmFinder();
                var best = await finder.FindPathAsync(map);

                var index = best.ListRoutes.FindIndex(o => o.DtChegada > DtEntrega);
                
                if(index == -1)
                    index = best.ListRoutes.Count;

                var list = best.ListRoutes.Where(o=> best.ListRoutes.IndexOf(o) < index);

                listroutes = best.ListRoutes.Where(o => best.ListRoutes.IndexOf(o) >= index).ToList();
                var listpoints = listroutes.Select(o => o.Destination).ToList();

                var listdest = map.Destinations.Where(o => !listpoints.Exists(a => a.Latitude == o.Latitude && a.Longitude == o.Longitude)).ToList();
                Entregadores.Add(new Entregador { Saida = map.Storage, Pontos = listdest });

                map.Destinations.RemoveAll(o=> !listpoints.Exists(a=> a.Latitude == o.Latitude && a.Longitude == o.Longitude));

            }

            sw.Stop();
            using (var color = new ConsoleFont(ConsoleColor.Red))
                Console.WriteLine($"Tempo ({sw.Elapsed.Minutes}:{sw.Elapsed.Seconds})...");

            sw.Reset();

            using (var color = new ConsoleFont(ConsoleColor.White))
            {
                Console.WriteLine($"Para Entregar em todos os destinos até {DtAgora} é preciso {Entregadores.Count} Entregadores...");
            }

            Console.WriteLine($"");
            while (Entregadores.Exists(o => o.Pontos.Any()))
            {
                sw.Start();
                foreach (var Entregador in Entregadores)
                {
                    map = new RouteMap(Entregador.Saida);

                    foreach (var item in Entregador.Pontos)
                        map.AddDestination(item);

                    if (!map.Destinations.Any())
                        continue;

                    var finder = new GeneticAlgorithmFinder();
                    var best = await finder.FindPathAsync(map);

                    Console.WriteLine($"Entregador ({Entregadores.IndexOf(Entregador) + 1})");
                    using (var color = new ConsoleFont(ConsoleColor.White))
                    {
                        Console.WriteLine($"Saindo: {best.Map.Storage.Date.ToString("dd/MM/yyy hh:mm")}");
                        Console.WriteLine($"Saia de {map.Storage.Name}...");
                        Console.WriteLine($"Vá para {best.ListRoutes.First().Destination.Name}");
                        Console.WriteLine($"Horario de Chegada: {best.ListRoutes.First().DtChegada.ToString("dd/MM/yyy hh:mm")}");
                    }
                    Console.WriteLine($"");
                    map.Next(best.ListRoutes);

                    Entregador.Saida = map.Storage;
                    Entregador.Pontos = map.Destinations;
                }
                sw.Stop();
                using (var color = new ConsoleFont(ConsoleColor.Red))
                    Console.WriteLine($"Tempo ({sw.Elapsed.Minutes}:{sw.Elapsed.Seconds})...");

                sw.Reset();
                SearchRoute.SaveCache();
                Console.ReadKey();
            }       
        }
    }
}
