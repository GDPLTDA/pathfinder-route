using Pathfinder.Abstraction;
using Pathfinder.CLI.Abstraction;
using Pathfinder.CLI.Viewer;
using System;
namespace Pathfinder.CLI.Factories
{
    public class ViewerFactory : IFactory<IViewer, ViewerEnum>
    {
        public static IViewer GetConsoleViewerImplementation()
         => new ConsoleViewer();
        public static IViewer GetOpenGlViewerImplementation(int blocksize = 0)
        {
            var result = new OpenGlViewer();

            if (blocksize > 0)
                result.BlockSize = blocksize;

            return result;
        }

        public IViewer GetImplementation(ViewerEnum option)
            => Decide(option);
        private static IViewer Decide(ViewerEnum option)
        {
            switch (option)
            {
                case ViewerEnum.Console:
                    return GetConsoleViewerImplementation();
                case ViewerEnum.OpenGL:
                    return GetOpenGlViewerImplementation();
            }
            throw new Exception("No viewer selected");
        }
    }
}
