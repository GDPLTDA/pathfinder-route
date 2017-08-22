using Pathfinder.Abstraction;
using Pathfinder.CLI.Abstraction;
namespace Pathfinder.CLI.Viewer
{
    public class OpenGlViewer : IViewer
    {
        OpenGlWindow window;
        IFinder _finder;
        public int BlockSize { get; set; } = 10;

        public OpenGlViewer()
        {
        }

        public void Run(IMap map, IHeuristic h)
        {
            window = new OpenGlWindow(map, _finder, h, BlockSize);
            window.Run();
        }
        public void SetFinder(IFinder finder)
        {
            _finder = finder;
        }
    }
}
