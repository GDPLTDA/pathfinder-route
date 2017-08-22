
using System;
namespace Pathfinder.Abstraction
{
    public interface IFinder
    {
        string Name { get; set; }
        int SleepUITimeInMs { get; set; }
        bool Find(IMap grid, IHeuristic heuristic);
        int Weight { get; set; }
        long GetProcessedTime();
        event EventHandler Start;
        event EventHandler Step;
        event EventHandler End;
    }
    public class FinderEventArgs : EventArgs
    {
        public long PassedTimeInMs { get; set; }
        public int Step { get; set; }
        public int ExpandedNodesCount { get; set; }
        public bool Finded { get; set; }
        public IMap GridMap { get; set; }
    }
}