using PathFinder.GeneticAlgorithm;
using PathFinder.Routes;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PathFinder.Tests
{
    public class UnitTest
    {

        static HttpClient httpClient;
        GASettings GASettings;
        IRouteService RouteService;
        public UnitTest()
        {
            httpClient = new HttpClient();
            GASettings = new GASettings();
            RouteService = new CachedGoogleService(httpClient);
        }


        [Theory(DisplayName = "Deve encontrar uma rota!")]
        [InlineData("Aceita.txt")]
        public async Task Deve_Encontrar_Rota(string fileName)
        {
            var result = await RunTest($"./Tests/{fileName}");

            Assert.Equal(TipoErro.Concluido, result);
        }
        [Theory(DisplayName = "Não deve encontrar uma rota!")]
        [InlineData("ErroTempo.txt")]
        public async Task Nao_Deve_Encontrar_Rota(string fileName)
        {
            var result = await RunTest($"./Tests/{fileName}");

            Assert.Equal(TipoErro.EstourouTempo, result);
        }
        [Theory(DisplayName = "Não tem entregadores suficientes para a rota!")]
        [InlineData("Limite.txt")]
        public async Task Nao_Tem_Entregadores(string fileName)
        {
            var result = await RunTest($"./Tests/{fileName}");

            Assert.Equal(TipoErro.LimiteEntregadores, result);
        }
        //[Theory(DisplayName = "Estoura o tempo limite!")]
        //[InlineData("Senacs 1.txt")]
        //public async Task Estoura_Tempo_Limite(string fileName)
        //{
        //    var result = await RunTest($"./Tests/{fileName}");

        //    Assert.Equal(TipoErro.EstourouTempoEntrega, result);
        //}
        public async Task<TipoErro> RunTest(string filename)
        {
            var config = await PRVJTFinder.GetConfigByFile(filename, RouteService);

            foreach (MutateEnum mut in Enum.GetValues(typeof(MutateEnum)))
            {
                foreach (CrossoverEnum cro in Enum.GetValues(typeof(CrossoverEnum)))
                {
                    // Altera a configuração do GA
                    GASettings.Mutation = mut;
                    GASettings.Crossover = cro;

                    // Carrega a configuração do roteiro
                    var finder = new PRVJTFinder(config, RouteService, GASettings);
                    // Executa a divisão de rotas
                    var result = await finder.Run();

                    if (result.Erro)
                        return result.TipoErro;

                    while (!result.Concluido)
                    {
                        foreach (var item in result.ListEntregadores)
                        {
                            if (item.NextRoute == null)
                                continue;
                            var entreresult = await finder.Step(item);

                            if (entreresult.Erro)
                                return result.TipoErro;
                        }
                    }
                }
            }
            return TipoErro.Concluido;
        }
    }
}
