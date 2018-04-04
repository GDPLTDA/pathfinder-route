using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VRP.GeneticAlgorithm.Delegates;
using VRP.GeneticAlgorithm.Models;

namespace VRP.GeneticAlgorithm
{
    public class Genome
    {
        public Local Origin { get; }
        public IList<Local> Locals { get; }
        public IReadOnlyCollection<Route> Routes { get; }
        public double Fitness { get; } = double.MaxValue;

        private IList<Local> AllLocals => EnumerableEx.Return(Origin).Concat(Locals).ToArray();

        public Genome(IEnumerable<Local> locals)
        {
            Origin = locals.First();
            Locals = locals.Skip(1).ToArray();
        }


        private Genome(IList<Local> locals, double fitness, IReadOnlyCollection<Route> routes)
        {
            Locals = locals;
            Fitness = fitness;
            Routes = routes;
            Origin = locals.First();
            Locals = locals.Skip(1).ToArray();
        }

        internal Genome Clone() => new Genome(AllLocals, Fitness, Routes);

        public Genome CalcFitness(FitnessDelegate func) =>
                    new Genome(
                            AllLocals,
                            func?.Invoke(this) ?? double.MaxValue, Routes
                       );

        public async Task<Genome> CalcRoutesAsync(Func<IEnumerable<Local>, Task<IReadOnlyCollection<Route>>> func) =>
            new Genome(AllLocals, Fitness, await func(AllLocals));

        public bool IsEqual(Genome genome) => genome.Fitness == Fitness;

        public override string ToString() => $"F={Fitness}";

    }
}