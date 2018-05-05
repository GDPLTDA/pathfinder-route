using CalcRoute.GeneticAlgorithm;
using CalcRoute.Routes;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalcRoute
{
    public enum TipoErro
    {
        [Description("Concluído!")]
        Concluido,
        [Description("Não é possível entregar a tempo!")]
        EstourouTempo,
        [Description("Limite de entregadores excedido!")]
        LimiteEntregadores,
        [Description("Tempo limite para a entrega foi excedido!")]
        EstourouTempoEntrega,
    }

    public class PRVJTFinder
    {
        private readonly IRouteService RouteService;
        readonly GeneticAlgorithmFinder GaFinder;
        readonly PRVJTConfig Config;
        readonly GASettings Settings;

        public PRVJTFinder(PRVJTConfig config, IRouteService routeService)
        {
            Config = config;
            GaFinder = new GeneticAlgorithmFinder(routeService, config.Settings);
            Settings = Config.Settings;
            RouteService = routeService;
        }

        public async Task<FinderResult> Run()
        {
            var result = new FinderResult();
            var map = Config.Map;

            using (TimeMeasure.Init())
            {

                var route = await GaFinder.FindPathAsync(map);
                result.ListEntregadores = route.Trucks.Where(e => e.Locals.Any()).ToList();
                result.TipoErro = (route.Trucks.SelectMany(e => e.Routes).Any(l => l.Late)) ? TipoErro.EstourouTempo : TipoErro.Concluido;
                result.BestGenome = route;
            }

            return result;
        }
        
        public static async Task<PRVJTConfig> GetConfigByFile(string fileName, IRouteService routeService, GASettings settings = null)
        {
            using (TimeMeasure.Init())
            {
                var config = new PRVJTConfig();
                using (var sr = new StreamReader(fileName, Encoding.GetEncoding("ISO-8859-1")))
                {
                    var name = ReadConfig("Nome", sr);
                    var endereco = ReadConfig("Endereco", sr);
                    var saida = DateTime.Parse(ReadConfig("Saida", sr));
                    var volta = DateTime.Parse(ReadConfig("Volta", sr));
                    var entregadores = Convert.ToInt32(ReadConfig("Entregadores", sr));
                    var descarga = Convert.ToInt32(ReadConfig("Descarga", sr));

                    config.Settings = settings ?? new GASettings();
                    config.Settings.NumberOfTrucks = entregadores;
                    config.Map = new Roteiro(routeService, name, endereco, saida, volta);
                    config.Map.DataSaida = saida;

                    config.DtLimite = volta;
                    //Linha de titulo
                    sr.ReadLine();

                    while (sr.Peek() >= 0)
                    {
                        var line = sr.ReadLine().Split("|").Select(o => o.Replace("\t", "")).ToList();

                        var map = new Local(line[0], line[1])
                        {
                            Period = new Period(line[2], line[3], descarga)
                        };

                        await config.Map.AddDestination(map);
                    }
                }
                return config;
            }
        }
        public static string ReadConfig(string configname, StreamReader st)
        {
            var line = st.ReadLine();

            var regex = Regex.Match(line, $"{configname}=([\\w\\s\\.\\,\\/\\:\\-]+)").Groups.Last();

            return regex.Value;
        }
    }
}
