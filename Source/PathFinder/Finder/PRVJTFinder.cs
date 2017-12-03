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
        [Description("Concluido!")]
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
                                        .Where(o => remainingPoints.Exists(a => a.Equals(o))).ToList();

                    var mapentr = new RouteMap(map);

                    destinosEntrega.ForEach(async e => await mapentr.AddDestination(e));

                    var g = new Genome { Map = mapentr, ListRoutes = routesInTime.Select(o => o).ToList(), ListPoints = remainingPoints.Select(o => o).ToList() };

                    g.CalcFitness(GaFinder.Fitness);

                    result.ListEntregadores.Add(new Entregador
                    {
                        Genome = g,
                        Numero = result.ListEntregadores.Count,
                        Map = mapentr,
                        NextRoute = routesInTime.First()
                    });

                    if (result.ListEntregadores.Count > Config.NumEntregadores)
                        return result.Register(TipoErro.LimiteEntregadores);

                    map.Destinations.RemoveAll(o => remainingPoints.Exists(a => a.Equals(o)));
                }
            }
            
            return result;
        }

        public async Task<EntregadorResult> Step(Entregador entregador)
        {
            using (TimeMeasure.Init())
            {
                var result = new EntregadorResult(entregador);
                var map = entregador.Map;

                if (!map.Destinations.Any())
                    return result;

                if (entregador.NextRoute.DtChegada > Config.DtLimite)
                    return result.Register(TipoErro.EstourouTempoEntrega);

                map.Next(entregador.Genome.ListRoutes);

                if (entregador.Genome.ListPoints.Any())
                    entregador.Genome.ListPoints.RemoveAt(0);

                if (!entregador.Genome.ListRoutes.Any())
                {
                    entregador.NextRoute = null;
                    return result;
                }

                var best = await GaFinder.FindPathAsync(map, entregador.Genome);

                entregador.Genome = new Genome(best);
                entregador.NextRoute = best.ListRoutes.FirstOrDefault();
                entregador.Map = map;

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
                    var name = ReadConfig("Nome", sr);
                    var endereco = ReadConfig("Endereco", sr);
                    var saida = DateTime.Parse(ReadConfig("Saida", sr));
                    var volta = DateTime.Parse(ReadConfig("Volta", sr));
                    var entregadores = Convert.ToInt32(ReadConfig("Entregadores", sr));
                    var descarga = Convert.ToInt32(ReadConfig("Descarga", sr));

                    config.Map = new RouteMap(name, endereco, saida, volta);
                    config.Map.DataSaida = saida;
                    
                    config.DtLimite = volta;
                    config.NumEntregadores = entregadores;
                    //Linha de titulo
                    sr.ReadLine();

                    while (sr.Peek() >= 0)
                    {
                        var line = sr.ReadLine().Split("|").Select(o => o.Replace("\t", "")).ToList();

                        var map = new MapPoint(line[0], line[1])
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
