using Pathfinder.Abstraction;
using Pathfinder.CLI.CommandParser;
using Pathfinder.Factories;
using System;
using System.IO;

namespace Pathfinder.CLI.Commands
{
    public class MapGenerationCommand
    {

        public static int RunMapGeneration(MapGenerationOption options)
        {
            IMapGenerator generator;



            generator = options.PatternLength > 0 ?
                            MapGeneratorFactory.GetStandardMapGeneratorImplementation(options.PatternLength)
                            : MapGeneratorFactory.GetRandomMapGeneratorImplementation();

            if (options.Qtd > 0)
            {
                var filename = Path.GetFileName(options.Filename);
                var ext = Path.GetExtension(options.Filename);


                if (string.IsNullOrEmpty(ext))
                    ext = "txt";

                for (int i = 0; i < options.Qtd; i++)
                {
                    options.Filename = i.ToString().PadLeft(options.Qtd.ToString().Length, '0') + filename + "." + ext;
                    GenerateMap(generator, options);
                    DrawTextProgressBar(i, options.Qtd);
                }
                DrawTextProgressBar(options.Qtd, options.Qtd);
            }
            else
            {
                GenerateMap(generator, options);
            }



            return 0;
        }


        static void GenerateMap(IMapGenerator generator, MapGenerationOption options)
        {
            var map = generator.DefineMap(
                          options.Diagonal,
                          options.Width,
                          options.Height,
                          options.Seed,
                          options.MinPathLength
                      );


            FileTool.SaveFileFromMap(map, options.Filename, options.Directory);

        }


        static void DrawTextProgressBar(int progress, int total, int barLength = 30, int left = 0, ConsoleColor color = ConsoleColor.Green)
        {
            if (total == 0)
                return;

            const char loadCchar = ' ';
            Console.CursorLeft = left;
            Console.Write("[");
            Console.CursorLeft = barLength + left + 1;
            Console.Write("]");
            Console.CursorLeft = 1 + left;
            var step = ((double)barLength / total);
            //draw filled part
            var position = 1;
            for (int i = 0; i < step * progress; i++)
            {
                Console.BackgroundColor = color;
                Console.CursorLeft = left + position++;
                Console.Write(loadCchar);
            }
            //draw unfilled part
            for (int i = position; i <= barLength; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = left + position++;
                Console.Write(loadCchar);
            }
            //draw totals
            Console.CursorLeft = left + barLength + 4;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(progress * 100 / total + "%    ");
        }
    }
}

