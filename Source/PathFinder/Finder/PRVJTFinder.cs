using PathFinder.GeneticAlgorithm;
using PathFinder.Routes;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PathFinder
{

    public enum TipoErro
    {
        [Description("Não é possível entregar a tempo!")]
        EstourouTempo,
        [Description("Limite de entregadores excedido!")]
        LimiteEntregadores,
        [Description("Tempo limite para a entrega foi excedido!")]
        EstourouTempoEntrega,
    }
    public class PRVJTFinder
    {
        readonly GeneticAlgorithmFinder GaFinder;
        readonly PRVJTConfig Config;

        public PRVJTFinder(PRVJTConfig config)
        {
            Config = config;
            GaFinder = new GeneticAlgorithmFinder();
        }

        public async Task<FinderResult> Run()
        {
            var result = new FinderResult();
            var map = Config.Map;

            using (TimeMeasure.Init())
            {
                while (map.Destinations.Any())
                {
                    var best = await GaFinder.FindPathAsync(map);

                    var routesInTime = best.ListRoutes.TakeWhile(e => e.DtChegada <= Config.DtLimite).ToList();

                    if (!routesInTime.Any())
                        return result.Register(TipoErro.EstourouTempo);

                    var remainingPoints = routesInTime.Select(o => o.Destination).ToList();

                    var destinosEntrega = map.Destinations
                                        .Where(o => remainingPoints.Exists(a => a.Equals(o)));

                    result.ListEntregadores.Add(new Entregador
                    {
                        Saida = map.Storage,
                        Pontos = destinosEntrega.ToList(),
                        NextRoute = routesInTime.First()
                    });

                    if (result.ListEntregadores.Count > Config.NumEntregadores)
                        return result.Register(TipoErro.LimiteEntregadores);

                    map.Destinations.RemoveAll(o => remainingPoints.Exists(a => a.Equals(o)));
                }
            }

            //SearchRoute.SaveCache();
            return result;
        }

        public async Task<EntregadorResult> Step(Entregador entregador)
        {
            using (TimeMeasure.Init())
            {
                var result = new EntregadorResult(entregador);
                var map = new RouteMap();
                await map.Start(entregador.Saida);

                entregador
                    .Pontos
                    .ForEach(async e => await map.AddDestination(e));

                if (!map.Destinations.Any())
                    return result;

                if (entregador.NextRoute.DtChegada > Config.DtLimite)
                    return result.Register(TipoErro.EstourouTempoEntrega);

                // Se existe só um ponto, esse ponto vira o estoque na proximo, então não é preciso
                if (entregador.Pontos.Count == 1)
                    return result;

                var best = await GaFinder.FindPathAsync(map, entregador.Genome);

                map.Next(best.ListRoutes);

                entregador.Genome = new Genome(best);
                entregador.NextRoute = best.ListRoutes.First();
                entregador.Saida = map.Storage;
                entregador.Pontos = map.Destinations;

                return result;
            }
        }
        public static async Task<PRVJTConfig> GetConfigByFile(string fileName)
        {

            using (TimeMeasure.Init())
            {
                var config = new PRVJTConfig();
                using (var sr = new StreamReader(fileName, Encoding.GetEncoding("ISO-8859-1")))
                {
                    var name = ReadConfig("Estoque", sr);
                    var endereco = ReadConfig("Endereco", sr);
                    var saida = DateTime.Parse(ReadConfig("Saida", sr));
                    var volta = DateTime.Parse(ReadConfig("Volta", sr));
                    var entregadores = Convert.ToInt32(ReadConfig("Entregadores", sr));
                    var descarga = ReadConfig("Descarga", sr);

                    config.Map = new RouteMap();
                    await config.Map.Start(new MapPoint(name, endereco), saida);

                    config.DtLimite = volta;
                    config.NumEntregadores = entregadores;
                    //Linha de titulo
                    sr.ReadLine();

                    while (sr.Peek() >= 0)
                    {
                        var line = sr.ReadLine().Split("|").Select(o => o.Replace("\t", "")).ToList();

                        var map = new MapPoint(line[0], line[1])
                        {
                            Period = new Period(line[2], line[3])
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
            return Regex.Match(line, $"{configname}=([\\w\\s\\.\\/\\:]+)").Groups.Last().Value;
        }
    }
}
