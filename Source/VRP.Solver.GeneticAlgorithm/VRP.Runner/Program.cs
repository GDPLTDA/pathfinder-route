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
            Console.WriteLine("Hello World!");

            var service = new GoogleService(httpClient);
            var ga = new GeneticAlgorithmFinder(
                    FitnessTimePath.Calc,
                    MutateSM.Apply,
                    CrossoverOBX.Make,
                    SelectionRouletteWheel.SelectCouple,
                    service,
                    new GASettings()
                );

            var address = new[] {
                   "Rua Maria Roschel Schunck 817",
                   "Avenida Manuel Alves Soares 460"
            };

            var locals = new List<Local>();
            foreach (var item in address)
                locals.Add(await service.GetPointAsync(item, ""));

            var response = await ga.Epoch(locals);

        }
    }
}
