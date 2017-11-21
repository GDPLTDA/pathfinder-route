using PathFinder.GeneticAlgorithm;
using PathFinder.Routes;
using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

namespace PathFinder
{
    public class PRVJTFinder
    {
        readonly GeneticAlgorithmFinder GaFinder;
        readonly PRVJTConfig Config;

        public PRVJTFinder(PRVJTConfig config)
        {
            Config = config;
            GaFinder = new GeneticAlgorithmFinder();
        }

        public async Task<FinderResult> Run()
        {
            var result = new FinderResult();
            var map = Config.Map;

            using (TimeMeasure.Init())
            {
                Print($"Dividindo Rotas...");

                while (map.Destinations.Any())
                {
                    var best = await GaFinder.FindPathAsync(map);

                    var routesInTime = best.ListRoutes.TakeWhile(e => e.DtChegada <= Config.DtLimite).ToList();

                    if (!routesInTime.Any())
                        return result.Register("Não é possível entregar a tempo!");

                    var remainingPoints = routesInTime.Select(o => o.Destination).ToList();

                    var destinosEntrega = map.Destinations
                                        .Where(o => remainingPoints.Exists(a => a.Equals(o)));

                    result.ListEntregadores.Add(new Entregador { Saida = map.Storage, Pontos = destinosEntrega.ToList(), NextRoute = routesInTime.First() });

                    if (result.ListEntregadores.Count > Config.NumEntregadores)
                        return result.Register("Limite de entregadores excedido!");

                    map.Destinations.RemoveAll(o => remainingPoints.Exists(a => a.Equals(o)));
                }
            }

            SearchRoute.SaveCache();
            return result;
        }

        public async Task<EntregadorResult> Step(Entregador entregador)
        {
            using (TimeMeasure.Init())
            {
                var result = new EntregadorResult(entregador);

                Print($"Calculando Rota do Entregador {entregador.Numero}...");

                var map = new RouteMap(entregador.Saida);

                entregador
                    .Pontos
                    .ForEach(e => map.AddDestination(e));

                if (!map.Destinations.Any())
                    return result;

                if (entregador.NextRoute.DtChegada > Config.DtLimite)
                    return result.Register("Tempo limite para a entrega foi excedido!");

                Print($"Saindo: {map.DataSaida: dd/MM/yyy hh:mm}");
                Print($"Saia de {map.Storage.Name}");
                Print($"Vá para {entregador.NextRoute.Destination.Name}");
                Print($"Horario de Chegada: {entregador.NextRoute.DtChegada:dd/MM/yyy hh:mm)}");

                // Se existe só um ponto, esse ponto vira o estoque na proximo, então não é preciso
                if (entregador.Pontos.Count == 1)
                    return result;

                var best = await GaFinder.FindPathAsync(map, entregador.Genome);

                map.Next(best.ListRoutes);

                entregador.Genome = new Genome(best);
                entregador.NextRoute = best.ListRoutes.First();
                entregador.Saida = map.Storage;
                entregador.Pontos = map.Destinations;

                return result;
            }
        }

        public static void Print(string message = null, ConsoleColor color = ConsoleColor.White)
        {
            using (new ConsoleFont(color))
                WriteLine(string.IsNullOrEmpty(message) ? "\n" : message);
        }

        public static void PrintErro(string message = null, ConsoleColor color = ConsoleColor.Red)
        {
            using (new ConsoleFont(color))
                WriteLine(string.IsNullOrEmpty(message) ? "\n" : message);
        }

    }
}
