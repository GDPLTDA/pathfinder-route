
using System;
using System.Diagnostics;
namespace Pathfinder.Abstraction
{
    public abstract class AbstractFinder : IFinder
    {
        public event EventHandler Step;
        public event EventHandler End;
        public event EventHandler Start;


        public int Weight { get; set; } = 1;
        protected Stopwatch _stopwatch { get; set; }

        public string Name { get; set; }
        public int SleepUITimeInMs { get; set; }

        public virtual long GetProcessedTime() => _stopwatch.ElapsedMilliseconds;


        protected AbstractFinder(string name)
        {
            Clear();
            Name = name;
        }


        protected void Clear()
        {
            _stopwatch = new Stopwatch();
        }
        protected virtual void OnStep(FinderEventArgs e)
        {
            _stopwatch.Stop();
            StepConfig(e.GridMap);
            e.GridMap.UpdateMaxNodes();
            Step?.Invoke(this, e);
            _stopwatch.Start();
        }
        public virtual void StepConfig(IMap map)
        {
        }
        protected virtual void OnEnd(FinderEventArgs e)
        {
            _stopwatch.Stop();
            End?.Invoke(this, e);
        }
        protected virtual void OnStart(FinderEventArgs e)
        {
            Start?.Invoke(this, e);
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        public abstract bool Find(IMap grid, IHeuristic heuristic);


        protected FinderEventArgs BuildArgs(int i, IMap map, bool finded = false)
        {
            var args = new FinderEventArgs
            {
                PassedTimeInMs = _stopwatch.ElapsedMilliseconds,
                Step = i,
                ExpandedNodesCount = map.ClosedListCount() + map.OpenListCount(),
                Finded = finded,
                GridMap = map
            };
            return args;
        }
    }
}