using PathFinder.Routes.GoogleMapas;
using System;
using System.Collections.Generic;
using System.Text;

namespace PathFinder.Routes
{
    public class MapPoint
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Date { get; set; }
        public Period Period { get; set; } = new Period();

        public MapPoint(string name, Location loc)
        {
            Name = name;
            Latitude = loc.lat;
            Longitude = loc.lng;
        }

        public override string ToString()
        {
            return $"({Latitude},{Longitude}) {Name}";
        }
    }
}
