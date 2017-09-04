using System;
using System.Collections.Generic;
using System.Text;

namespace PathFinder.Routes
{
    public class MapPoint
    {
        public string Name { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }

        public MapPoint(string name) { Name = name; }

        public override string ToString()
        {
            return $"({Latitude},{Longitude}) {Name}";
        }
    }
}
