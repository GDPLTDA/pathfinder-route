using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VRP.GeneticAlgorithm;
using VRP.GeneticAlgorithm.Models;

namespace VRP.Runner
{
    class Program
    {
        static HttpClient httpClient = new HttpClient();

        static async Task Main()
        {
            Console.WriteLine("Calc routes:");

            var service = new CachedGoogleService(httpClient);
            var ga = new GeneticAlgorithmFinder(
                    Fitness.FitnessTimePath,
                    Mutates.MutateSM,
                    Crossover.CrossoverOBX,
                    Selection.SelectCouple,
                    service,
                    new GASettings()
                );

            var address = new[] {
                   "Rua Maria Roschel Schunck 817",
                   "Avenida Manuel Alves Soares 460",
                   "Lambda3",
                   "Senac Santo Amaro",
            };

            var locals = new List<Local>();
            foreach (var item in address)
                locals.Add(await service.GetPointAsync(item, item));

            var response = await ga.Epoch(locals);

        }
    }
}
