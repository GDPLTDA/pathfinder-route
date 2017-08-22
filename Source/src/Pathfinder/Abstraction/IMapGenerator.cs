namespace Pathfinder.Abstraction
{
    public interface IMapGenerator
    {
        IMap DefineMap(DiagonalMovement diagonal, int width, int height, int seed, int minPathLength);
        IMap DefineMap();
    }
}