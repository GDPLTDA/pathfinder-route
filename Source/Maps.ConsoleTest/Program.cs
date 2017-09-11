using System;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using PathFinder.Routes;
using System.Threading.Tasks;

namespace Maps.ConsoleTest
{
    class Program
    {
        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

        static async Task MainAsync(string[] args)
        {
            var search = new SearchRoute();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1 Route");

            var route = await search.GetRouteAsync("São Paulo", "Rio de Janeiro");
            ShowRoute(route);

            Console.WriteLine();
            Console.WriteLine("Multi Routes");

            var routes = await search.GetRoutesAsync("Curitiba", "São Paulo", "Rio de Janeiro", "Belo Horizonte", "Vitória", "Salvador BA", "Fortaleza");

            foreach (var item in routes)
                ShowRoute(item);

            Console.ReadKey();
        }

        static void ShowRoute(Route route)
        {
            if (route.Meters > 0)
            {
                var disformat = $"{route.Km:n2} km";
                var durformat = $"{route.Hours:n2} horas)";

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Origem: {route.Origin} Destino: {route.Destination} Distancia: {disformat} Duração: {durformat}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Origem: {route.Origin} Destino: {route.Destination} - Erro!");
            }
        }
    }
}