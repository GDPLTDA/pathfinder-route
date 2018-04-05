using ColoredConsole;
using PathFinder.GeneticAlgorithm;
using PathFinder.Routes;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PathFinder
{
    class Program
    {
        static HttpClient httpClient = new HttpClient();

        static async Task Main()
        {
            Print("Carregando rotas de teste...");
            var service = new CachedGoogleService(httpClient);
            service.LoadCache();

            var config = await PRVJTFinder.GetConfigByFile("./Tests/Senacs.txt", service);
            var finder = new PRVJTFinder(config, service, new GASettings());

            Print($"Dividindo Rotas...");
            var result = await finder.Run();

            service.SaveCache();

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
                    if (item.NextRoute == null)
                        continue;

                    Print($"Calculando Rota do Entregador {item.Numero}...");

                    Print($"Saindo: {item.Map.DataSaida: dd/MM/yyy HH:mm}");
                    Print($"Saia de ({item.Saida.Name}){item.Saida.Endereco}");
                    Print($"Vá para ({item.NextRoute.Destino.Name}){item.NextRoute.Destino.Endereco}");
                    Print($"Horario de Chegada: {item.NextRoute.DhChegada:dd/MM/yyy HH:mm)}");
                    Print($"Espera: {item.NextRoute.Destino.Period.Descarga} Minutos");

                    var entreresult = await finder.Step(item);

                    if (entreresult.Erro)
                        PrintErro(entreresult.Messagem);
                }
                Print("Proxima rota de todos os entregadores?");
                Console.ReadKey();
            }

            Print("Finalizado!");
            Console.ReadKey();
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
