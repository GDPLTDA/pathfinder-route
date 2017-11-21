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
            PRVJTFinder.Print("Carregando rotas de teste...");
            var map = new RouteMap("São Paulo SP", DateTime.Now);

            using (TimeMeasure.Init())
            {
                File.ReadAllText("Capitais.txt", Encoding.GetEncoding("iso-8859-1"))
                    .Replace("\r", string.Empty).Split("\n")
                    .ToList()
                    .ForEach(e => map.AddDestination(e));
            }

            var config = new PRVJTConfig { Map = map, DtInicio = DateTime.Now, NumEntregadores = 10 };

            var finder = new PRVJTFinder(config);
            var result = await finder.Run();

            if (result.Erro)
            {
                PRVJTFinder.PrintErro(result.Messagem);
                Console.ReadKey();
                return;
            }

            while (!result.Concluido)
            {
                foreach (var item in result.ListEntregadores)
                {
                    var entreresult = await finder.Step(item);

                    if (!entreresult.Erro)
                        PRVJTFinder.PrintErro(entreresult.Messagem);
                }
                Console.WriteLine("Proxima rota de todos os entregadores?");
                Console.ReadKey();
            }

        }
    }
}
