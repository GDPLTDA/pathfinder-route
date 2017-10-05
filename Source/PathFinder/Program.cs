using PathFinder.GeneticAlgorithm;
using PathFinder.Routes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace PathFinder
{
    class Program
    {
        static List<Entregador> Entregadores { get; set; } = new List<Entregador>();

        static DateTime DtAgora { get; set; } = DateTime.Now;
        static DateTime DtEntrega { get; set; } = DateTime.Now.AddDays(4);

        static async Task Main()
        {
            RouteMap map = null;

            using (TimeMeasure.Init())
            {
                Print($"Buscando Endereços...");

                map = new RouteMap("São Paulo SP", DtAgora);

                File.ReadAllText("Capitais.txt", Encoding.GetEncoding("iso-8859-1"))
                    .Replace("\r", string.Empty).Split("\n")
                    .ToList()
                    .ForEach(e => map.AddDestination(e));
            }

            using (TimeMeasure.Init())
            {
                Print($"Dividindo Rotas...");

                while (map.Destinations.Any())
                {
                    var finder = new GeneticAlgorithmFinder();
                    var best = await finder.FindPathAsync(map);

                    var routesInTime = best.ListRoutes.TakeWhile(e => e.DtChegada <= DtEntrega).ToList();

                    var remainingRoutes = best.ListRoutes
                                                    .AsEnumerable().Reverse()
                                                    .TakeWhile(e => e.DtChegada > DtEntrega)
                                                    .ToList();

                    var remainingPoints = remainingRoutes.Select(o => o.Destination).ToList();

                    var destinosEntrega = map.Destinations
                                        .Where(o => !remainingPoints
                                                        .Exists(a => a.Latitude == o.Latitude && a.Longitude == o.Longitude)
                                                  ).ToList();

                    Entregadores.Add(new Entregador { Saida = map.Storage, Pontos = destinosEntrega });

                    map.Destinations.RemoveAll(o => !remainingPoints.Exists(a => a.Latitude == o.Latitude && a.Longitude == o.Longitude));

                }
            }

            Print($"Para Entregar em todos os destinos até {DtAgora} é preciso {Entregadores.Count} Entregadores...");

            while (Entregadores.Exists(o => o.Pontos.Any()))
            {
                using (TimeMeasure.Init())
                {
                    foreach (var Entregador in Entregadores)
                    {
                        map = new RouteMap(Entregador.Saida);

                        Entregador
                            .Pontos
                            .ForEach(e => map.AddDestination(e));

                        if (!map.Destinations.Any())
                            continue;

                        var finder = new GeneticAlgorithmFinder();
                        var best = await finder.FindPathAsync(map);

                        Print($"Entregador ({Entregadores.IndexOf(Entregador) + 1})");
                        Print($"Saindo: {best.Map.Storage.Date:dd/MM/yyy hh:mm}");
                        Print($"Saia de {map.Storage.Name}...");
                        Print($"Vá para {best.ListRoutes.First().Destination.Name}");
                        Print($"Horario de Chegada: {best.ListRoutes.First().DtChegada:dd/MM/yyy hh:mm)}");
                        Print();

                        map.Next(best.ListRoutes);

                        Entregador.Saida = map.Storage;
                        Entregador.Pontos = map.Destinations;
                    }
                }

                SearchRoute.SaveCache();
                ReadKey();
            }
        }

        static void Print(string message = null, ConsoleColor color = ConsoleColor.White)
        {
            using (new ConsoleFont(color))
                WriteLine(string.IsNullOrEmpty(message) ? "\n" : message);
        }
    }
}
