using MoreLinq;
using CalcRoute.GeneticAlgorithm;
using CalcRoute.Routes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CalcRoute.DataGenerator
{
    class Program
    {
        static void Main()
        {
            var arquivoDados = new FileStream("resultados.csv", FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.WriteThrough);

            using (var writer = new StreamWriter(arquivoDados))
            {
                writer.WriteLine("Msg;Arquivo;Entregadores;Mutation;Cross;Fitness");

                var itens = Directory
                 .GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Tests\"))
                 .Select(f => RunTest(f).Result)
                 .Select(r => r.ToDelimitedString(string.Empty))
                ;

                MoreEnumerable.ForEach(itens, e => Console.WriteLine(e));
            }
        }
        public async static Task<IEnumerable<Result>> RunTest(string filename)
        {
            var ret = new List<Result>();
            var http = new HttpClient();
            var settings = new GASettings();
            var routeService = new GoogleMatrixService(http);

            foreach (MutateEnum mut in Enum.GetValues(typeof(MutateEnum)))
            {
                foreach (CrossoverEnum cro in Enum.GetValues(typeof(CrossoverEnum)))
                {
                    // Altera a configuração do GA
                    settings.Mutation = mut;
                    settings.Crossover = cro;

                    for (int i = 0; i < 10; i++)
                    {
                        var file = Path.GetFileName(filename);
                        Console.WriteLine($"A:{file} I{i} M:{mut} C:{cro}");
                        var config = await PRVJTFinder.GetConfigByFile(filename, routeService);
                        // Carrega a configuração do roteiro
                        var finder = new PRVJTFinder(config, routeService);
                        // Executa a divisão de rotas
                        var result = await finder.Run();

                        if (result.Erro)
                        {
                            ret.Add(new Result(
                                     result.TipoErro,
                                     filename,
                                     -1,
                                     mut,
                                     cro,
                                    0
                                 ));
                            continue;
                        }

                        ret.Add(new Result(
                                result.TipoErro,
                                filename,
                                result.ListEntregadores.Count(),
                                mut,
                                cro,
                                result.BestGenome.Fitness
                            ));
                    }
                }
            }
            http.Dispose();
            return ret;
        }
    }
}
