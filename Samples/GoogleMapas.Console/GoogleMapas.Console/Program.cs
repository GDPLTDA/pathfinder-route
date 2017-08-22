using System;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using PathFinder.Routes;

namespace GoogleMapas.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var search = new SearchRoute();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1 Route");

            var route = search.GetRoute("São Paulo", "Rio de Janeiro");
	        ShowRoute(route);				

            Console.WriteLine();
            Console.WriteLine("Multi Routes");

            var routes = search.GetRoutes("Curitiba", "São Paulo", "Rio de Janeiro", "Belo Horizonte", "Vitória", "Salvador BA", "Fortaleza");

            foreach (var item in routes)
                ShowRoute(item);

            Console.ReadKey();
        }

        static void ShowRoute(Route route)
        {
            if (route.Sucess)
            {
                var disformat = string.Format("{0:n2} km", route.Km);
                var durformat = string.Format("{0:n2} horas)", route.Hours);

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