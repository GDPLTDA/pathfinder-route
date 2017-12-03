﻿using MoreLinq;
using PathFinder.GeneticAlgorithm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                Directory
                 .GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Tests\"))
                 .Select(f => RunTest(f).Result)
                 .Select(r => r.ToDelimitedString(";"))
                 .ForEach(e => writer.WriteLine(e));

            }
        }
        public async static Task<IEnumerable<Result>> RunTest(string filename)
        {
            var config = await PRVJTFinder.GetConfigByFile(filename);
            var ret = new List<Result>();

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
                    var result = await finder.Run();

                    if (result.Erro)
                        ret.Add(new Result(
                                 result.TipoErro,
                                 filename,
                                 -1,
                                 mut,
                                 cro,
                                 result.ListEntregadores.Sum(e => e.Genome.Fitness)
                             ));

                    while (!result.Concluido)
                    {
                        foreach (var item in result.ListEntregadores)
                        {
                            if (item.NextRoute == null)
                                continue;
                            var entreresult = await finder.Step(item);

                            if (entreresult.Erro)
                                ret.Add(new Result(
                                         result.TipoErro,
                                         filename,
                                         -1,
                                         mut,
                                         cro,
                                         result.ListEntregadores.Sum(e => e.Genome.Fitness)
                                 ));
                        }
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
            return ret;
        }

    }
}
