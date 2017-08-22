using Pathfinder.Abstraction;
using Pathfinder.MapGenerators;
using System;
namespace Pathfinder.Factories
{
    public class MapGeneratorFactory : IFactory<IMapGenerator, MapGeneratorEnum>
    {
        public static IMapGenerator GetFileMapGeneratorImplementation(string filename = "")
        {
            var result = new FileMapGenerator
            {
                FileName = filename
            };


            return result;
        }
        public static IMapGenerator GetStaticMapGeneratorImplementation()
            => new StaticMapGenerator();
        public static IMapGenerator GetRandomMapGeneratorImplementation()
            => new RandomMapGenerator();
        public static IMapGenerator GetStandardMapGeneratorImplementation(int paternSize = 0)
        {
            var result = new StandardMapGenerator();

            if (paternSize > 0)
                result.Blocksize = paternSize;

            return result;
        }
        public IMapGenerator GetImplementation(MapGeneratorEnum option)
        => Decide(option);
        private static IMapGenerator Decide(MapGeneratorEnum option)
        {
            switch (option)
            {
                case MapGeneratorEnum.File:
                    return GetFileMapGeneratorImplementation();
                case MapGeneratorEnum.Static:
                    return GetStaticMapGeneratorImplementation();
                case MapGeneratorEnum.Random:
                    return GetRandomMapGeneratorImplementation();
                case MapGeneratorEnum.WithPattern:
                    return GetStandardMapGeneratorImplementation();
            }
            throw new Exception("No generator selected");
        }
    }
}