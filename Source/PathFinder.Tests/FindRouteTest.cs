using FluentAssertions;
using System.IO;
using System.Text;
using Xunit;

namespace PathFinder.Tests
{
    public class UnitTest
    {
        [Theory(DisplayName = "Should find a route")]
        [InlineData("Teste 1.txt")]
        [InlineData("Teste 2.txt")]
        [InlineData("Teste 3.txt")]
        [InlineData("Teste 4.txt")]
        [InlineData("Teste 5.txt")]
        public void ShouldFindARoute(string fileName)
        {
            var filedata = File.ReadAllText($"./Tests/{fileName}", Encoding.GetEncoding("ISO-8859-1"));


            filedata.Should().NotBeNullOrWhiteSpace();
        }
    }
}
