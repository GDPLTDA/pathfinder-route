﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VRP.GeneticAlgorithm.Delegates;
using VRP.GeneticAlgorithm.Models;

namespace VRP.GeneticAlgorithm
{
    public class GeneticAlgorithmFinder
    {
        private readonly IRouteService routeService;

        public FitnessDelegate Fitness { get; }
        public MutationDelegate Mutation { get; }
        public CrossOverDelegate Crossover { get; }
        public SelectionDelegate Selection { get; }

        private readonly GASettings settings;

        internal static readonly Random Random = new Random();

        protected int ParallelQuantity = 1;

        public GeneticAlgorithmFinder(
                FitnessDelegate fitnessDelegate,
                MutationDelegate mutationDelegate,
                CrossOverDelegate crossOverDelegate,
                SelectionDelegate selectionDelegate,
                IRouteService routeService,
                GASettings gASettings
            )
        {
            Fitness = fitnessDelegate;
            Mutation = mutationDelegate;
            Crossover = crossOverDelegate;
            Selection = selectionDelegate;
            settings = gASettings;
            this.routeService = routeService;
        }

        public async Task<Genome> Epoch(IEnumerable<Local> locals)
        {
            if (new Delegate[] { Fitness, Mutation, Crossover, Selection, }.Any(e => e == null))
                throw new Exception("GA cant run without all operators");

            var config = new GASettings();

            var crossOverRate = config.CrossoverRate;
            var mutationRate = config.MutationRate;

            locals = EnumerableEx.Return(locals.First()).Concat(locals.Skip(1).Shuffle());

            var population = await Enumerable.Range(0, settings.PopulationSize)
                             .Select(_ => new Genome(locals))
                             .ToObservable()
                             .Select(n => Observable.FromAsync(_ => n.CalcRoutesAsync(routeService.CalcFullRoute)))
                             .Merge(ParallelQuantity)
                             .Select(e => e.CalcFitness(Fitness))
                             .ToList();



            var lastGen = await
                  GetNewPopulationCollectionAsync(population, crossOverRate, mutationRate)
                 .ToObservable()
                 .SelectMany(n => Observable.FromAsync(_ => n))
                 .Take(settings.GenerationLimit)
                 .LastAsync()
               ;


            return lastGen.OrderBy(e => e.Fitness).First();
        }


        private IEnumerable<Task<IEnumerable<Genome>>> GetNewPopulationCollectionAsync(IEnumerable<Genome> populations, double crossOverRate, double mutationRate)
        {
            while (true)
                yield return GetNextPopulationAsync(populations, crossOverRate, mutationRate);
        }

        private async Task<IEnumerable<Genome>> GetNextPopulationAsync(IEnumerable<Genome> populations, double crossOverRate, double mutationRate) =>
           await populations
                .Take(settings.BestSolutionToPick)
                .Concat(
                    Enumerable.Range(0, settings.PopulationSize - settings.BestSolutionToPick)
                    .Select(e => Selection(populations.ToList()))
                    .Select(e => Crossover(crossOverRate, e.dad, e.mon))
                    .SelectMany(e => e.ToArray())
                    .Take(settings.PopulationSize)
                    .Select(e => Mutation(mutationRate, e))
                    .Select(e => new Genome(e.Locals))
                )
                .OrderBy(e => e.Fitness)
                .ToObservable(NewThreadScheduler.Default)
                .Select(n => Observable.FromAsync(_ => n.CalcRoutesAsync(routeService.CalcFullRoute)))
                .Merge(ParallelQuantity)
                .Select(e => e.CalcFitness(Fitness))
                .ToList();


    }
}