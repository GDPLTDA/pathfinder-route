using PathFinder.Routes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PathFinder.GeneticAlgorithm.Core
{
    public class TruckCollection : IList<Local>
    {
        public IList<Truck> Trucks;

        public int TruckCount => Trucks.Count;

        public TruckCollection(IEnumerable<Truck> trucks)
        {
            Trucks = trucks
                        .ToList()
                        .Select(e => e.ClearRoutes())
                        .OrderByDescending(t => t.Locals.Count > 0).ToList();
        }

        public int Count => CountLocals * TruckCount;
        public int CountLocals => Trucks.SelectMany(t => t.Locals).Distinct().Count();
        public bool IsReadOnly => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public bool Contains(Local item) => Trucks.Any(t => t.Locals.Contains(item));

        private void ShrinkTruks() => Trucks = Trucks.OrderByDescending(t => t.Locals.Count > 0).ToList();

        public IEnumerator<Local> GetEnumerator() =>
            (from a in Trucks
             from b in a.Locals
             select b).GetEnumerator();


        public Local this[int index]
        {
            get
            {
                try
                {
                    var (truckIndex, localIndex) = GetTrueIndex(index);

                    var truck = Trucks[truckIndex];

                    if (!truck.Locals.Any())
                        throw new Exception("invalid index at TruckCollection");

                    var ret = truck.Locals[localIndex];

                    return ret;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            set
            {
                try
                {
                    var (truckIndex, localIndex) = GetTrueIndex(index);

                    var truck = Trucks[truckIndex];

                    if (!truck.Locals.Any())
                        throw new Exception("invalid index at TruckCollection");

                    var ret = truck.Locals[localIndex];
                    ret = value;
                    ShrinkTruks();

                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        public void Insert(int index, Local item)
        {
            var (tIndex, lIndex) = GetTrueIndex(index);
            Trucks[tIndex].Locals.Insert(lIndex, item);
        }
        public void RemoveAt(int index)
        {
            var (tIndex, lIndex) = GetTrueIndex(index);
            Trucks[tIndex].Locals.RemoveAt(lIndex);
            ShrinkTruks();
        }


        private (int truck, int local) GetTrueIndex(int index)
        {

            try
            {
                var truckIndex = index / TruckCount;
                var localIndex = index - (truckIndex * CountLocals);

                var validTruckCount = Trucks.Count(t => t.Locals.Count > 0);

                var normalizedTruckIndex = (int)((truckIndex * validTruckCount) / ((double)TruckCount));
                var localCount = Trucks[normalizedTruckIndex].Locals.Count;

                var normalizedIndex = (int)((localIndex * localCount) / (double)CountLocals);

                return (normalizedTruckIndex, normalizedIndex);
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        #region notImplemented
        public void Add(Local item) => throw new NotImplementedException();
        public void CopyTo(Local[] array, int arrayIndex) => throw new NotImplementedException();
        public int IndexOf(Local item) => throw new NotImplementedException();
        public bool Remove(Local item) => throw new NotImplementedException();
        public void Clear() => throw new NotImplementedException();
        #endregion
    }
}
