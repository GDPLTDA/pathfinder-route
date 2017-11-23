using ColoredConsole;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PathFinder
{
    class Program
    {
        static async Task Main()
        {
            Print("Carregando rotas de teste...");
            //var map = new RouteMap("São Paulo SP", DateTime.Now);

            //using (TimeMeasure.Init())
            //{
            //    File.ReadAllText("Capitais.txt", Encoding.GetEncoding("iso-8859-1"))
            //        .Replace("\r", string.Empty).Split("\n")
            //        .ToList()
            //        .ForEach(e => map.AddDestination(e));
            //}

            var config = await PRVJTFinder.GetConfigByFile("Senacs 1.txt"); //new PRVJTConfig { Map = map, NumEntregadores = 7 };

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
                    if (!item.Pontos.Any())
                        continue;

                    Print($"Calculando Rota do Entregador {item.Numero}...");

                    Print($"Saindo: {item.Saida.Date: dd/MM/yyy hh:mm}");
                    Print($"Saia de ({item.Saida.Name}){item.Saida.Endereco}");
                    Print($"Vá para ({item.NextRoute.Destination.Name}){item.NextRoute.Destination.Endereco}");
                    Print($"Horario de Chegada: {item.NextRoute.DtChegada:dd/MM/yyy hh:mm)}");

                    var entreresult = await finder.Step(item);

                    if (!entreresult.Erro)
                        PrintErro(entreresult.Messagem);
                }

                Print("Proxima rota de todos os entregadores?");
                Console.ReadKey();
            }
        }
        public static void Print(string message = null) =>
               ColorConsole.WriteLine(
                   (string.IsNullOrEmpty(message) ? "\n" : message).White()
                   );

        public static void PrintErro(string message) =>
                ColorConsole.WriteLine(
                   (string.IsNullOrEmpty(message) ? "\n" : message).Red().OnDarkRed()
                   );
    }
}
