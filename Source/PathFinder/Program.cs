using PathFinder.Routes;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder
{
    class Program
    {
        static async Task Main()
        {
            Print("Carregando rotas de teste...");
            var map = new RouteMap("São Paulo SP", DateTime.Now);

            using (TimeMeasure.Init())
            {
                File.ReadAllText("Capitais.txt", Encoding.GetEncoding("iso-8859-1"))
                    .Replace("\r", string.Empty).Split("\n")
                    .ToList()
                    .ForEach(e => map.AddDestination(e));
            }

            var config = new PRVJTConfig { Map = map, DtInicio = DateTime.Now, NumEntregadores = 7 };

            var finder = new PRVJTFinder(config);

            Print($"Dividindo Rotas...");
            var result = await finder.Run();

            if (result.Erro)
            {
                PrintErro(result.Messagem);
                Console.ReadKey();
                return;
            }
            Print($"Numero de Entregadores {result.ListEntregadores.Count}...");

            while (!result.Concluido)
            {
                foreach (var item in result.ListEntregadores)
                {
                    Print($"Calculando Rota do Entregador {item.Numero}...");

                    Print($"Saindo: {map.DataSaida: dd/MM/yyy hh:mm}");
                    Print($"Saia de {map.Storage.Name}");
                    Print($"Vá para {item.NextRoute.Destination.Name}");
                    Print($"Horario de Chegada: {item.NextRoute.DtChegada:dd/MM/yyy hh:mm)}");

                    var entreresult = await finder.Step(item);

                    if (!entreresult.Erro)
                        PrintErro(entreresult.Messagem);
                }

                Print("Proxima rota de todos os entregadores?");
                Console.ReadKey();
            }
        }
        public static void Print(string message = null, ConsoleColor color = ConsoleColor.White)
        {
            using (new ConsoleFont(color))
                Console.WriteLine(string.IsNullOrEmpty(message) ? "\n" : message);
        }

        public static void PrintErro(string message = null, ConsoleColor color = ConsoleColor.Red)
        {
            using (new ConsoleFont(color))
                Console.WriteLine(string.IsNullOrEmpty(message) ? "\n" : message);
        }
    }
}
