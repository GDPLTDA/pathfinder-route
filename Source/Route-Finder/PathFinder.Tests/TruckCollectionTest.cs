using FluentAssertions;
using PathFinder.GeneticAlgorithm;
using PathFinder.GeneticAlgorithm.Core;
using PathFinder.Routes;
using System.Linq;
using Xunit;

namespace PathFinder.Tests
{
    public class TruckCollectionTest
    {
        [Fact]
        public void DeveGerarCollectionCorreta()
        {
            var locals1 = new[] {
                new Local("Casa 1"),
                new Local("Casa 2"),
                new Local("Casa 3"),
            };

            var locals2 = new[] {
                new Local("Casa 4"),
                new Local("Casa 5"),
                new Local("Casa 6"),
            };
            var locals3 = new[] {
                new Local("Casa 7"),
                new Local("Casa 8"),
                new Local("Casa 9"),
            };

            var trucks = new[] {
                new Truck(locals1),
                new Truck(locals2),
                new Truck(locals3),
            };

            var collec = new TruckCollection(trucks);

            Local[] Triplique(Local l) =>
                Enumerable.Repeat(l, 3).ToArray();

            var equivalent = new[] {
                   Triplique(locals1[0]),
                   Triplique(locals1[1]),
                   Triplique(locals1[2]),

                   Triplique(locals2[0]),
                   Triplique(locals2[1]),
                   Triplique(locals2[2]),

                   Triplique(locals3[0]),
                   Triplique(locals3[1]),
                   Triplique(locals3[2]),
            }.SelectMany(e => e).ToArray();

            for (int i = 0; i < collec.Count; i++)
                collec[i].Should().Be(equivalent[i]);

        }
    }
}
