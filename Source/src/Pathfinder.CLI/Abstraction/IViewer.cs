using Pathfinder.Abstraction;
namespace Pathfinder.CLI.Abstraction
{
    public interface IViewer
    {
        void Run(IMap map, IHeuristic h);
        void SetFinder(IFinder finder);
    }
}
