using MoreLinq;
using PathFinder.GeneticAlgorithm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PathFinder.DataGenerator
{
    class Program
    {
        static FileStream arquivoDados;
        static StreamWriter writer;
        static Program()
        {
            arquivoDados = new FileStream("resultados.csv", FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.WriteThrough);
            writer = new StreamWriter(arquivoDados);
        }

        static void Main() =>
                    Directory
                      .GetFiles(@".\Tests\")
                      .Select(f => RunTest(f))
                      .Select(r => r.ToDelimitedString(";") + "\n")
                      .ForEach(e => writer.Write(e));


        public static IEnumerable<Result> RunTest(string filename)
        {
            var config = PRVJTFinder.GetConfigByFile(filename).Result;

            foreach (MutateEnum mut in Enum.GetValues(typeof(MutateEnum)))
            {
                foreach (CrossoverEnum cro in Enum.GetValues(typeof(CrossoverEnum)))
                {
                    // Altera a configuração do GA
                    GASettings.Mutation = mut;
                    GASettings.Crossover = cro;

                    // Carrega a configuração do roteiro
                    var finder = new PRVJTFinder(config);
                    // Executa a divisão de rotas
                    var result = finder.Run().Result;

                    if (result.Erro)
                        yield return new Result(
                                 result.TipoErro,
                                 filename,
                                 -1,
                                 mut,
                                 cro,
                                 result.ListEntregadores.Sum(e => e.Genome.Fitness)
                             );

                    while (!result.Concluido)
                    {
                        foreach (var item in result.ListEntregadores)
                        {
                            if (item.NextRoute == null)
                                continue;
                            var entreresult = finder.Step(item).Result;

                            if (entreresult.Erro)
                                yield return new Result(
                                         result.TipoErro,
                                         filename,
                                         -1,
                                         mut,
                                         cro,
                                         result.ListEntregadores.Sum(e => e.Genome.Fitness)
                                 );
                        }
                    }

                    yield return new Result(
                            result.TipoErro,
                            filename,
                            result.ListEntregadores.Count(),
                            mut,
                            cro,
                            result.ListEntregadores.Sum(e => e.Genome.Fitness)
                        );
                }
            }
        }

    }
}
