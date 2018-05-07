using CalcRoute.GeneticAlgorithm;
using CalcRoute.Routes;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CalcRoute.DataGenerator
{
    internal class Program
    {
        private static void Main()
        {
            var arquivoDados = new FileStream("resultados.csv", FileMode.Create, FileAccess.Write, FileShare.Read, 4096, FileOptions.WriteThrough);

            using (var writer = new StreamWriter(arquivoDados))
            {
                writer.WriteLine("Msg;Indice;Arquivo;Entregadores;Mutation;Cross;Fitness");

                var files = Directory
                 .GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Tests\"));

                foreach (var file in files)
                {
                    var result = RunTest(file).Result;
                    var csv = result.ToDelimitedString(";");

                    writer.WriteLine(csv);
                }
            }
        }

        public async static Task<IEnumerable<Result>> RunTest(string filename)
        {
            var ret = new List<Result>();
            var http = new HttpClient();
            var routeService = new CachedGoogleMatrixService(http);

            foreach (MutateEnum mut in Enum.GetValues(typeof(MutateEnum)))
            {
                foreach (CrossoverEnum cro in Enum.GetValues(typeof(CrossoverEnum)))
                {
                    var settings = new GASettings
                    {
                        // Altera a configuração do GA
                        Mutation = mut,
                        Crossover = cro
                    };

                    for (int i = 0; i < 1; i++)
                    {
                        var file = Path.GetFileName(filename);
                        Console.WriteLine($"A:{file} I:{i} M:{mut} C:{cro}");
                        var config = await PRVJTFinder.GetConfigByFile(filename, routeService, settings);
                        // Carrega a configuração do roteiro
                        var finder = new PRVJTFinder(config, routeService);
                        // Executa a divisão de rotas

                        routeService.LoadCache();
                        var result = await finder.Run();
                        routeService.SaveCache();

                        if (result.Erro)
                        {
                            ret.Add(new Result(
                                     result.TipoErro,
                                     i,
                                     filename,
                                     result.ListEntregadores.Count(),
                                     mut,
                                     cro,
                                    result.BestGenome.Fitness
                                 ));
                            continue;
                        }

                        ret.Add(new Result(
                                result.TipoErro,
                                i,
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