using FluentAssertions;
using System.IO;
using System.Text;
using Xunit;

namespace PathFinder.Tests
{
    public class UnitTest
    {
        [Theory(DisplayName = "Deve encontrar uma rota!")]
        [InlineData("Senacs 1.txt")]
        [InlineData("Extras 1.txt")]
        public void Deve_Encontrar_Rota(string fileName)
        {
            var filedata = File.ReadAllText($"./Tests/{fileName}", Encoding.GetEncoding("ISO-8859-1"));


            filedata.Should().NotBeNullOrWhiteSpace();
        }
        [Theory(DisplayName = "Não deve encontrar uma rota!")]
        [InlineData("MacDonalts.txt")]
        public void Nao_Deve_Encontrar_Rota(string fileName)
        {
            var filedata = File.ReadAllText($"./Tests/{fileName}", Encoding.GetEncoding("ISO-8859-1"));


            filedata.Should().NotBeNullOrWhiteSpace();
        }
        [Theory(DisplayName = "Não tem entregadores suficientes para a rota!")]
        [InlineData("Senacs 2.txt")]
        [InlineData("Extras 2.txt")]
        [InlineData("MacDonalts.txt")]
        public void Nao_Tem_Entregadores(string fileName)
        {
            var filedata = File.ReadAllText($"./Tests/{fileName}", Encoding.GetEncoding("ISO-8859-1"));


            filedata.Should().NotBeNullOrWhiteSpace();
        }
        [Theory(DisplayName = "Estoura o tempo limite!")]
        [InlineData("Senacs 1.txt")]
        [InlineData("MacDonalts.txt")]
        public void Estoura_Tempo_Limite(string fileName)
        {
            var filedata = File.ReadAllText($"./Tests/{fileName}", Encoding.GetEncoding("ISO-8859-1"));


            filedata.Should().NotBeNullOrWhiteSpace();
        }
    }
}
