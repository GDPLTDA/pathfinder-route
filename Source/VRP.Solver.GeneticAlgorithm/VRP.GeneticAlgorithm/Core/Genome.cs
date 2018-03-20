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
        public IList<Local> Locals { get; }
        public IReadOnlyCollection<Route> Routes { get; }
        public double Fitness { get; } = double.MaxValue;

        public Genome(IEnumerable<Local> locals)
        {
            Locals = locals.ToArray();
        }


        private Genome(IList<Local> locals, double fitness, IReadOnlyCollection<Route> routes)
        {
            Locals = locals;
            Fitness = fitness;
            Routes = routes;
        }


        internal Genome Clone() => new Genome(Locals, Fitness, Routes);

        public Genome CalcFitness(FitnessDelegate func) => new Genome(Locals, func?.Invoke(this) ?? double.MaxValue, Routes);
        public async Task<Genome> CalcRoutesAsync(Func<IEnumerable<Local>, Task<IReadOnlyCollection<Route>>> func) => new Genome(Locals, Fitness, await func(Locals));

        public bool IsEqual(Genome genome) => genome.Fitness == Fitness;

        public override string ToString() => $"F={Fitness}";

    }
}