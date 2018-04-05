using MoreLinq;
using PathFinder.GeneticAlgorithm;
using PathFinder.Routes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PathFinder.DataGenerator
{
    class Program
    {
        static void Main()
        {
            var arquivoDados = new FileStream("resultados.csv", FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.WriteThrough);

            using (var writer = new StreamWriter(arquivoDados))
            {
                writer.WriteLine("Msg;Arquivo;Entregadores;Mutation;Cross;Fitness");

                Directory
                 .GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Tests\"))
                 .Select(f => RunTest(f).Result)
                 .Select(r => r.ToDelimitedString(string.Empty))
                 .ForEach(e => writer.WriteLine(e));
            }
        }
        public async static Task<IEnumerable<Result>> RunTest(string filename)
        {
            var ret = new List<Result>();
            var http = new HttpClient();
            var settings = new GASettings();
            var routeService = new CachedGoogleService(http);

            foreach (MutateEnum mut in Enum.GetValues(typeof(MutateEnum)))
            {
                foreach (CrossoverEnum cro in Enum.GetValues(typeof(CrossoverEnum)))
                {
                    var config = await PRVJTFinder.GetConfigByFile(filename, routeService);
                    // Altera a configuração do GA
                    settings.Mutation = mut;
                    settings.Crossover = cro;
                    // Carrega a configuração do roteiro
                    var finder = new PRVJTFinder(config, routeService, settings);
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
                                 result.ListEntregadores.Sum(e => e.Genome.Fitness)
                             ));
                        continue;
                    }

                    ret.Add(new Result(
                            result.TipoErro,
                            filename,
                            result.ListEntregadores.Count(),
                            mut,
                            cro,
                            result.ListEntregadores.Sum(e => e.Genome.Fitness)
                        ));
                }
            }
            http.Dispose();
            return ret;
        }
    }
}
