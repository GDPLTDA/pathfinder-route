using CalcRoute.GeneticAlgorithm;
using CalcRoute.GeneticAlgorithm.Abstraction;
using CalcRoute.GeneticAlgorithm.Crossover;
using CalcRoute.Routes;
using FakeItEasy;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace PathFinder.Tests
{
    public class SubRouteInsertionCrossoverTests
    {
        [Fact]
        public void Should_Transfer_subroutes()
        {
            var settings = new GASettings();
            var service = A.Fake<IRouteService>();
            var random = A.Fake<IRandom>();
            var map = A.Dummy<Roteiro>();
            var locals = Enumerable.Range(0, 6).Select(e => new Local(e.ToString()) { Latitude = e, Longitude = e }).ToArray();

            A.CallTo(() => service.GetRouteAsync(A<Local>._, A<Local>._))
                .Returns(new Rota { Metros = 1 });

            A.CallTo(() => service.GetRouteAsync(locals[1], locals[4]))
                .Returns(new Rota { Metros = 0 }).Once();

            A.CallTo(() => service.GetRouteAsync(locals[3], locals[2]))
                .Returns(new Rota { Metros = 0 }).Once();

            A.CallTo(() => random.Next(A<int>._, A<int>._))
                .Returns(1);

            A.CallTo(() => random.Next(A<int>._, A<int>._))
                .ReturnsNextFromSequence(
                  // mon to dad
                  0 // indice da rota a extrair a subrota
                , 2 // quantidade de locais a extrais da rota
                , 1 // indice do primeiro local da subrota

                // dad to mon
                , 1 // indice da rota a extrair a subrota
                , 1 // quantidade de locais a extrais da rota
                , 0 // indice do primeiro local da subrota
                );

            var mon = new Genome(map, settings)
            {
                Trucks = new[] {
                    new Truck {
                        Locals = new[] { locals[0],locals[1],locals[2] }
                    },
                    new Truck {
                        Locals = new[] { locals[3],locals[4],locals[5] }
                    }

                }
            };

            var dad = new Genome(map, settings)
            {
                Trucks = new[] {
                    new Truck {
                        Locals = new[] { locals[5],locals[4],locals[3] }
                    },
                    new Truck {
                        Locals = new[] { locals[3],locals[2],locals[1] }
                    }

                }
            };


            var cx = new SubRouteInsertionCrossover(settings, random, service);
            var result = cx.Make(mon, dad);

            result.First().Trucks[0].Locals.Should()
                    .BeEquivalentTo(new[] { locals[5], locals[4], locals[1], locals[2], locals[3] });

            result.Last().Trucks[0].Locals.Should()
                    .BeEquivalentTo(new[] { locals[0], locals[1], locals[2], locals[3] });

        }

    }
}
