using PathFinder.GeneticAlgorithm.Abstraction;
using System;
using System.Linq;

namespace PathFinder.GeneticAlgorithm
{
    public class FitnessTimePath : IFitness
    {
        public double Calc(IGenome genome, GASettings settings)
        {
            try
            {

                var trucks = genome.Locals.Trucks;
                var totalMetros = trucks.Sum(t => t.Routes.Sum(r => r.Metros) + (t.DepotBack?.Metros ?? 0D));

                var totalEsperaDepois = 0D;
                var totalEsperaAntes = 0D;

                foreach (var truck in trucks)
                {
                    var start = genome.Map.DataSaida;
                    var finish = start;


                    foreach (var route in truck.Routes.Concat(new[] { truck.DepotBack }))
                    {
                        if (route == null)
                            continue;

                        var date = finish.AddMinutes(route.Minutos);
                        var from = CreateDateTime(date, route.Destino.Period.From);
                        var to = CreateDateTime(date, route.Destino.Period.To);

                        route.Descarga = route.Destino.Period.Descarga * 60;

                        if (date > to)
                        {
                            totalEsperaDepois += (date - to).TotalSeconds;
                            date = date.AddDays(1);
                            date = new DateTime(date.Year, date.Month, date.Day, from.Hour, from.Minute, from.Second);
                        }

                        if (date < from)
                        {
                            route.Espera = (from - date).TotalSeconds;
                            totalEsperaAntes += route.Espera;
                            date = date.Add(from - date);
                        }

                        route.DhSaida = finish;
                        route.DhChegada = date;
                        finish = date.AddMinutes(route.Destino.Period.Descarga);

                    }

                }
                return totalMetros +
                        (totalEsperaDepois * settings.ArriveAfterPenalty) +
                        (totalEsperaAntes * settings.ArriveBeforePenalty);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        static DateTime CreateDateTime(DateTime date, TimeSpan time) =>
            new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
    }
}




//public double Calc(IGenome genome)
//{
//    var start = genome.Map.DataSaida;
//    var finish = start;

//    foreach (var item in genome.Locals)
//    {
//        var date = finish.AddMinutes(item.Minutos);
//        var from = CreateDateTime(date, item.Destino.Period.From);
//        var to = CreateDateTime(date, item.Destino.Period.To);

//        // Converte para segundos
//        item.Descarga = item.Destino.Period.Descarga * 60;

//        if (date > to)
//        {
//            date = date.AddDays(1);
//            date = new DateTime(date.Year, date.Month, date.Day, from.Hour, from.Minute, from.Second);
//        }

//        if (date < from)
//        {
//            item.Espera = (from - date).TotalSeconds;
//            date = date.Add(from - date);
//        }

//        item.DhSaida = finish;
//        item.DhChegada = date;
//        finish = date.AddMinutes(item.Destino.Period.Descarga);
//    }
//    var totaltime = new TimeSpan(finish.Ticks - start.Ticks);

//    return genome.Trucks.Sum(o => o.Metros) + totaltime.Minutes;
//}

//DateTime CreateDateTime(DateTime date, TimeSpan time)
//{
//    return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
//}
