using FakeItEasy;
using FluentAssertions;
using PathFinder.GeneticAlgorithm;
using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.GeneticAlgorithm.Mutation;
using PathFinder.Routes;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PathFinder.Tests
{
    public class MutationTests
    {

        [Fact]
        public void SwapMutation()
        {
            var settings = new GASettings();
            var random = A.Fake<IRandom>();
            var map = A.Dummy<Roteiro>();
            var locals = GetLocals(4);

            const int
                indexTruck1 = 0,
                indexTruck2 = 1,
                indexLocal1 = 0,
                indexLocal2 = 1;

            A.CallTo(() => random.Next(A<int>._, A<int>._)).ReturnsNextFromSequence(indexTruck1, indexTruck2, indexLocal1, indexLocal2);

            var gen = new Genome(map, settings)
            {
                Trucks = new[] {
                    new Truck {
                        Locals = new[]{ locals[0], locals[1] }
                    },
                    new Truck {
                        Locals = new[]{ locals[2], locals[3] }
                    }

                }
            };

            var mutate = new SwapMutation(settings, random);
            var newGen = mutate.Apply(gen);

            newGen.Trucks[indexTruck1].Locals[indexLocal1]
                .Should().BeEquivalentTo(
                    gen.Trucks[indexTruck2].Locals[indexLocal2],
                    o => o.WithStrictOrdering());

            newGen.Trucks[indexTruck2].Locals[indexLocal2]
                .Should().BeEquivalentTo(
                    gen.Trucks[indexTruck1].Locals[indexLocal1],
                    o => o.WithStrictOrdering());
        }

        [Fact]
        public void InversionMutation()
        {
            var settings = new GASettings();
            var random = A.Fake<IRandom>();
            var map = A.Dummy<Roteiro>();
            var locals = GetLocals(5);

            const int
                indexTruck = 0,
                indexLocalStart = 1,
                length = 3;

            A.CallTo(() => random.Next(A<int>._, A<int>._))
                .ReturnsNextFromSequence(
                    indexTruck,
                    indexLocalStart,
                    length
                );

            var gen = new Genome(map, settings)
            {
                Trucks = new[] {
                    new Truck {
                        Locals = locals
                    }
                }
            };

            var mutate = new InversionMutation(settings, random);
            var newGen = mutate.Apply(gen);

            var genLocals = newGen.Trucks[indexTruck].Locals;

            var current = genLocals.Skip(indexLocalStart).Take(length).ToArray();
            var expect = locals.Skip(indexLocalStart).Take(length).Reverse().ToArray();


            current.Should().BeEquivalentTo(expect, o => o.WithStrictOrdering());

        }

        static IList<Local> GetLocals(int qtd) =>
             Enumerable.Range(0, qtd)
             .Select(e => new Local(e.ToString()) { Latitude = e, Longitude = e })
             .ToArray();


    }
}
