using CalcRoute;
using CalcRoute.Routes;
using FluentAssertions;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PathFinder.Tests
{
    public class UnitTest
    {

        static HttpClient httpClient;
        CachedGoogleMatrixService RouteService;
        public UnitTest()
        {
            httpClient = new HttpClient();
            RouteService = new CachedGoogleMatrixService(httpClient);
        }

        [Theory(DisplayName = "Deve calcular a rota e retornar um estado valido")]
        [InlineData("Aceita.txt", TipoErro.Concluido)]
        [InlineData("ErroTempo.txt", TipoErro.EstourouTempo)]
        //   [InlineData("Limite.txt", TipoErro.LimiteEntregadores)]
        public async Task Deve_Encontrar_Rota(string fileName, TipoErro estado)
        {
            var result = await RunTest($"./Tests/{fileName}");

            result.Should().Be(estado);
        }

        public async Task<TipoErro> RunTest(string filename)
        {
            RouteService.LoadCache();
            var config = await PRVJTFinder.GetConfigByFile(filename, RouteService);

            foreach (MutateEnum mut in Enum.GetValues(typeof(MutateEnum)))
                foreach (CrossoverEnum cro in Enum.GetValues(typeof(CrossoverEnum)))
                {
                    // Altera a configuração do GA
                    config.Settings.Mutation = mut;
                    config.Settings.Crossover = cro;

                    // Carrega a configuração do roteiro
                    var finder = new PRVJTFinder(config, RouteService);
                    // Executa a divisão de rotas
                    RouteService.LoadCache();
                    var result = await finder.Run();
                    RouteService.SaveCache();

                    if (result.Erro)
                        return result.TipoErro;

                    //while (!result.Concluido)
                    //{
                    //    foreach (var item in result.ListEntregadores)
                    //    {
                    //        if (item.NextRoute == null)
                    //            continue;
                    //        var entreresult = await finder.Step(item);

                    //        if (entreresult.Erro)
                    //            return result.TipoErro;
                    //    }
                    //}
                }

            return TipoErro.Concluido;
        }
    }
}
