using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PathFinder.Routes
{
    public class GoogleMatrixService : CachedGoogleDirectionsService, IRouteService
    {

        public GoogleMatrixService(HttpClient httpClient) : base(httpClient) { }

        Dictionary<string, Dictionary<string, Func<Rota>>> routes;

        public override async Task Prepare(IEnumerable<Local> locals)
        {
            routes = new Dictionary<string, Dictionary<string, Func<Rota>>>();
            var localsArray = locals.ToArray();
            var url = GetRequestMatrixUrl(locals, locals);
            dynamic data = JsonConvert.DeserializeObject(await httpClient.GetStringAsync(url));

            var rows = data.rows;
            for (int i = 0; i < rows.Count; i++)
            {
                var cols = rows[i].elements;
                var destinos = new Dictionary<string, Func<Rota>>();
                for (int j = 0; j < cols.Count; j++)
                {
                    var col = cols[j];
                    double metros = col.distance.value;
                    double segundos = col.duration.value;

                    var rotaFactory = Unclocure(localsArray[i], localsArray[j], metros, segundos);

                    destinos.Add(ParseLocal(localsArray[j]), rotaFactory);

                }
                routes.Add(ParseLocal(localsArray[i]), destinos);
            }

        }

        static Func<Rota> Unclocure(Local origem, Local destino, double metros, double segundos) =>
            () => new Rota
            {
                Origem = origem,
                Destino = destino,
                Metros = metros,
                Segundos = segundos,
            };

        public override Task<Rota> GetRouteAsync(Local origin, Local destination)
        {
            if (routes == null)
                throw new Exception("No initialized data");

            try
            {
                var rota = routes[ParseLocal(origin)][ParseLocal(destination)]();

                return Task.FromResult(rota);
            }
            catch (Exception)
            {

                throw;
            }

        }


        string GetRequestMatrixUrl(IEnumerable<Local> ori, IEnumerable<Local> des)
           => $"{Url}distancematrix/json?" +
               $"origins={string.Join("|", ori.Select(ParseLocal))}&" +
               $"destinations={string.Join("|", des.Select(ParseLocal))}&" +
               $"sensor=false&key={Key}";

    }
}