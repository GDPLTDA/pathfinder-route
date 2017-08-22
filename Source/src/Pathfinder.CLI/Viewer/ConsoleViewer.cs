using Pathfinder.Abstraction;
using Pathfinder.CLI.Abstraction;
using System;
using System.Threading;
namespace Pathfinder.CLI.Viewer
{
    public class ConsoleViewer : AbstractViewer
    {

        public override void Start()
        {
            Console.Clear();
        }
        public override void Loop(FinderEventArgs e)
        {
            Console.Clear();
            var text = FileTool.GetTextRepresentation(e.GridMap, true);
            Console.WriteLine(text);
            ShowStepLog(_finder, e);
            Thread.Sleep(_finder.SleepUITimeInMs);
        }
        public override void End(FinderEventArgs e)
        {
            Console.Clear();
            var path = e.GridMap.GetPath();
            var text = FileTool.GetTextRepresentation(e.GridMap, false, path);
            Console.WriteLine(text);
            ShowEndLog(_finder, path, e);
        }

        public override void Run(IMap map, IHeuristic h)
        {
            _finder.Find(map, h);
        }
    }
}
