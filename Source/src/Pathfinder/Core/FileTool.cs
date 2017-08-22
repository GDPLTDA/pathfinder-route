using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pathfinder
{
    public class FileTool
    {
        public static Char Start = 'S';
        public static Char End = 'E';
        public static Char Wall = '#';
        public static Char Path = '*';
        public static Char Empty = '-';
        public static Char Opened = 'O';
        public static Char Closed = 'C';

        public FileTool()
        {
        }
        public static string GetTextRepresentation(IMap map)
        {
            var ret = string.Empty;
            var builderRet = new System.Text.StringBuilder();
            builderRet.Append(ret);
            for (int i = 0; i < map.Height; i++)
            {
                var builder = new System.Text.StringBuilder();
                builder.Append(ret);
                for (int j = 0; j < map.Width; j++)
                {
                    var c = ' ';
                    var node = map[i, j];
                    c = node == map.StartNode ? Start : node == map.EndNode ? End : !node.Walkable ? Wall : Empty;
                    builder.Append(c.ToString());
                }
                builder.Append("\n");
                builderRet.Append(builder.ToString());
            }
            ret = builderRet.ToString();
            ret = ret.Remove(ret.LastIndexOf("\n"));
            return ret;
        }


        public static string GetTextRepresentation(IMap map, bool showOpenNodes, IEnumerable<Node> path = null)
        {
            var ret = string.Empty;
            for (int i = 0; i < map.Height; i++)
            {
                for (int j = 0; j < map.Width; j++)
                {
                    var c = ' ';
                    var node = map[i, j];
                    if (node == map.StartNode)
                        c = Start;
                    else if (node == map.EndNode)
                        c = End;
                    else if (path != null && path.ToList().Contains(node))
                        c = Path;
                    else if (!node.Walkable)
                        c = Wall;
                    else if (showOpenNodes && map.IsClosed(node))
                        c = Closed;
                    else if (showOpenNodes && map.IsOpen(node))
                        c = Opened;
                    else
                        c = Empty;
                    ret += c.ToString();
                }
                ret += "\n";
            }
            return ret;
        }

        public static IMap ReadMapFromFile(string fileName)
        {
            var width = 0;
            var height = 0;
            var nodes = new List<Node>();
            Node startNode = null;
            Node endNode = null;
            var x = 0;
            var y = 0;
            byte dig;


            var mapVars = new Dictionary<string, string>();


            if (fileName == "")
                throw new Exception("fileName is empty!");

            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                using (var reader = new BinaryReader(fs))
                {
                    while (!(reader.BaseStream.Position == reader.BaseStream.Length))
                    {
                        dig = reader.ReadByte();
                        if (dig == 13)
                            continue;
                        if (dig == 10)
                        {
                            y++;
                            x = 0;
                            continue;
                        }
                        var chrDig = (char)dig;
                        if (chrDig == '?')
                        {
                            var line = new List<char>();
                            while (chrDig != 10)
                                line.Add(chrDig = reader.ReadChar());
                            mapVars = ReadMapSettings(string.Join("", line), mapVars);
                            continue;
                        }
                        if (chrDig == Start)
                            startNode = new Node(x, y);
                        else
                        if (chrDig == End)
                            endNode = new Node(x, y);
                        else
                        if (chrDig == Wall)
                            nodes.Add(new Node(x, y) { Walkable = false });
                        else
                        if (chrDig == Empty)
                            nodes.Add(new Node(x, y) { Walkable = true });
                        else
                            throw new Exception("invalid character " + chrDig.ToString());
                        x++;
                    }
                    y++;
                }
            }
            width = x;
            height = y;

            DiagonalMovement? d = null;
            MapGeneratorEnum? t = MapGeneratorEnum.File;

            foreach (var item in mapVars)
            {
                switch (item.Key)
                {
                    case "diagonal":
                        d = (DiagonalMovement)Enum.Parse(typeof(DiagonalMovement), item.Value);
                        break;

                    case "type":
                        t = (MapGeneratorEnum)Enum.Parse(typeof(MapGeneratorEnum), item.Value);
                        break;

                    default:
                        break;
                }
            }


            if (d == null)
                throw new Exception("No diagonal seted on mapfile");

            var ret = new Map(d.Value, width, height)
            {
                StartNode = startNode,
                EndNode = endNode,
                MapType = t.Value
            };

            ret.DefineAllNodes(nodes);
            ret.DefineNode(ret.StartNode);
            ret.DefineNode(ret.EndNode);
            if (!ret.ValidMap())
                throw new Exception("Invalid map configuration");
            return ret;
        }
        private static Dictionary<string, string> ReadMapSettings(string line, Dictionary<string, string> vars)
        {

            var split = line.Split('=');
            var key = split[0].Replace("?", "");
            var value = split[1].Replace(";", "");

            vars.Add(key, value);

            return vars;

        }
        public static void SaveFileFromMap(IMap map, string filename, string directoryname = "")
        {
            var text = GetTextRepresentation(map);

            if (!string.IsNullOrEmpty(directoryname))
            {
                if (!Directory.Exists(directoryname))
                    Directory.CreateDirectory(directoryname);

            }

            var now = DateTime.Now;
            if (string.IsNullOrEmpty(filename))
            {
                filename = $"map_{map.MapType}_{map.Diagonal}_{map.Width}x{map.Height}_{now.Year}{now.Month}{now.Day}_{now.Hour}-{now.Minute}-{now.Second}.txt";
            }
            text = $"?diagonal={map.Diagonal};\n{text}";
            text = $"?type={map.MapType};\n{text}";
            File.WriteAllText(System.IO.Path.Combine(directoryname, filename), text);
        }
    }
}