﻿using Pathfinder.Abstraction;
using Pathfinder.CLI.CommandParser;
using Pathfinder.Factories;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pathfinder.CLI.Commands
{
    public class BatchCommand
    {
        public static int RunBatch(BatchOption option)
        {

            Process(option);

            Console.WriteLine("\n\nComplete...");
            return 0;

        }


        static void Process(BatchOption option)
        {
            var ft = new FileTool();
            var now = DateTime.Now;
            var folder = option.Directory;
            var dataFile = "_data.csv";
            var dataFileGA = "_dataGA.csv";

            var files = Directory.GetFiles(folder, "*.txt");
            var fileCount = files.Count();
            var finders = option.Finder.ToArray();
            var heuristics = option.Heuristic.ToArray();
            var Mutation = option.Mutate.ToArray();
            var Crossover = option.Crossover.ToArray();
            var Fitness = option.Fitness.ToArray();
            var Selection = option.Selection.Cast<SelectionEnum>().ToArray();

            var newdatafile = !File.Exists(dataFile);
            var newdatafileGA = !File.Exists(dataFileGA);

            StreamWriter csvFile;
            StreamWriter csvGAFile;
            if (newdatafile)
                csvFile = File.CreateText(dataFile);
            else
                csvFile = File.AppendText(dataFile);
            
            if (newdatafileGA)
                csvGAFile = File.CreateText(dataFileGA);
            else
                csvGAFile = File.AppendText(dataFileGA);

            //csvFile = new StreamWriter(File.Open(dataFile, FileMode.OpenOrCreate), Encoding.UTF8, 4096, false);
            //csvGAFile = new StreamWriter(File.Open(dataFileGA, FileMode.OpenOrCreate), Encoding.UTF8, 4096, false);

            if (newdatafile)
                csvFile.Write(new TextWrapper().GetHeader());
            if (newdatafileGA)
                csvGAFile.Write(new TextGAWrapper().GetHeader());

            for (int i = 0; i < fileCount; i++)
            {
                var map = FileTool.ReadMapFromFile(files[i]);
                foreach (var _finder in finders)
                {
                    foreach (var _h in heuristics)
                    {
                        var h = new HeuristicFactory().GetImplementation(_h);
                        var finder = new FinderFactory().GetImplementation(_finder);

                        if (finder is IGeneticAlgorithm)
                        {
                            for (int cross = 0; cross < Crossover.Count(); cross++)
                                for (int mut = 0; mut < Mutation.Count(); mut++)
                                    for (int fit = 0; fit < Fitness.Count(); fit++)
                                        for (int sel = 0; sel < Selection.Count(); sel++)
                                            for (int j = 0; j < option.GaTimes; j++)
                                            {
                                                GC.Collect();
                                                GC.WaitForPendingFinalizers();

                                                var GAFinder = (IGeneticAlgorithm)new FinderFactory().GetImplementation(_finder);
                                                GAFinder.Crossover = new CrossoverFactory().GetImplementation(Crossover[cross]);
                                                GAFinder.Mutate = new MutateFactory().GetImplementation(Mutation[mut]);
                                                GAFinder.Fitness = new FitnessFactory().GetImplementation(Fitness[fit]);
                                                GAFinder.Selection = new SelectionFactory().GetImplementation(Selection[sel]);

                                                var helper = $"\n                n:{j},cx:{GAFinder.Crossover.GetType().Name},m:{GAFinder.Mutate.GetType().Name},f:{GAFinder.Fitness.GetType().Name},s:{GAFinder.Selection.GetType().Name}";
                                                var csv = new TextWrapper();
                                                csv = RunStep(csv, i, fileCount, map, h, GAFinder, option.Directory, Path.GetFileName(files[i]), helper);
                                                csvFile.Write(csv.ToString());
                                                var csvGA = new TextGAWrapper
                                                {
                                                    Alg = csv.Alg,
                                                    MapNumber = csv.MapNumber,
                                                    Heuristic = csv.Heuristic,
                                                    MapTypeGenerate = csv.MapTypeGenerate,
                                                    MapDiagonal = csv.MapDiagonal,
                                                    MapSize = csv.MapSize,
                                                    Solution = csv.Solution,
                                                    Time = csv.Time,
                                                    MaxNodes = csv.MaxNodes,
                                                    PathLength = csv.PathLength,
                                                    Crossover = GAFinder.Crossover.GetType().Name,
                                                    Mutation = GAFinder.Mutate.GetType().Name,
                                                    Fitness = GAFinder.Fitness.GetType().Name,
                                                    Selection = GAFinder.Selection.GetType().Name,
                                                    Generations = GAFinder.Generations.ToString(),
                                                };
                                                csvGAFile.Write(csvGA.ToString());
                                            }
                        }
                        else
                        {
                            var csv = new TextWrapper();
                            csv = RunStep(csv, i, fileCount, map, h, finder, option.Directory, Path.GetFileName(files[i]));
                            csvFile.Write(csv.ToString());
                        }
                        csvFile.Flush();
                        csvGAFile.Flush();
                    }
                }
            }
            DrawTextProgressBar(fileCount, fileCount);

            csvFile.Dispose();
            csvGAFile.Dispose();

        }


        static TextWrapper RunStep(TextWrapper baseScv, int i, int fileCount, IMap map, IHeuristic h, IFinder finder, string path, string mapName, string plus = "")
        {
            var csv = baseScv;
            csv.MapNumber = i.ToString();
            csv.MapTypeGenerate = map.MapType.ToString();
            csv.Alg = finder.Name;
            csv.Heuristic = h.GetType().Name;
            csv.MapDiagonal = map.Diagonal.ToString();
            csv.MapSize = $"{map.Width}x{map.Height}";

            Console.CursorLeft = 0;
            if (Console.CursorTop > 0)
            {
                Console.Write(new string(' ', 80));
                Console.CursorLeft = 0;
            }
            Console.WriteLine($"            ({i}) {csv.Alg} - { csv.Heuristic } - {csv.MapDiagonal} ({csv.MapSize}/{csv.MapTypeGenerate})");
            DrawTextProgressBar(i, fileCount);
            if (finder.Find(map, h))
            {
                csv.PathLength = map.GetPath().OrderBy(x => x.G).Last().G.ToString();
                Console.ForegroundColor = ConsoleColor.Green;
                csv.Solution = "Yes";
            }
            else
            {
                csv.Solution = "No";
                csv.PathLength = "-1";
                Console.ForegroundColor = ConsoleColor.Red;
            }
            map.Clear();

            csv.Time = finder.GetProcessedTime().ToString();
            csv.MaxNodes = map.GetMaxExpandedNodes().ToString();
            Console.CursorTop -= 1;
            Console.CursorLeft = 0;
            Console.WriteLine($"{csv.Solution}-{csv.Time}ms");
            Console.ForegroundColor = ConsoleColor.White;

            // save solutions 
            var solutionPath = Path.Combine(path, "solutions", map.MapType.ToString(), finder.GetType().Name, h.GetType().Name);
            var fileName = mapName;

            if (finder is IGeneticAlgorithm ga)
            {
                solutionPath = Path.Combine(solutionPath, ga.Fitness.GetType().Name, ga.Selection.GetType().Name);
                fileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{ga.Mutate.GetType().Name}_{ga.Crossover.GetType().Name}_{i}.txt";

            }

            if (!Directory.Exists(solutionPath))
                Directory.CreateDirectory(solutionPath);

            var text = FileTool.GetTextRepresentation(map, false, map.GetPath());
            File.WriteAllText(Path.Combine(solutionPath, fileName), text);

            return csv;
        }
        class TextWrapper : BaseTextWrapper
        {
            public string MapNumber { get; set; }
            public string MapTypeGenerate { get; set; }
            public string MapSize { get; set; }
            public string MapDiagonal { get; set; }
            public string Alg { get; set; }
            public string Heuristic { get; set; }
            public string Solution { get; set; }
            public string Time { get; set; }
            public string MaxNodes { get; set; }
            public string PathLength { get; set; }
        }
        class TextGAWrapper : TextWrapper
        {
            public string Fitness { get; set; }
            public string Mutation { get; set; }
            public string Crossover { get; set; }
            public string Selection { get; set; }
            public string Generations { get; set; }
        }
        abstract class BaseTextWrapper
        {
            public string GetHeader()
            {
                var ret = new StringBuilder();
                var props = GetType().GetProperties();
                foreach (var item in props)
                {
                    ret.Append(item.Name);
                    ret.Append(";");
                }
                return ret.ToString() + "\n";
            }
            public override string ToString()
            {
                var ret = new StringBuilder();
                var type = GetType();
                var props = type.GetProperties();
                foreach (var item in props)
                {
                    var prop = type.GetProperty(item.Name);
                    ret.Append(prop.GetValue(this, null).ToString());
                    ret.Append(";");
                }
                return ret.ToString() + "\n";
            }
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