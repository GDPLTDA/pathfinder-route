using Pathfinder.Abstraction;
using System.IO;

namespace Pathfinder.MapGenerators
{
    public class FileMapGenerator : IMapGenerator
    {
        public string FileName { get; set; }

        public IMap DefineMap()
        {
            if (!File.Exists(FileName))
                throw new System.Exception("File not founded");

            var ft = new FileTool();
            var map = FileTool.ReadMapFromFile(FileName);
            return map;
        }

        public IMap DefineMap(DiagonalMovement diagonal, int width, int height, int seed, int minPathLength)
        {
            return DefineMap();
        }
    }
}



