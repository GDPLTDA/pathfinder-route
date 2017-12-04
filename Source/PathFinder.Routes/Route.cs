using System;
namespace PathFinder.Routes
{
    public class Route
    {
        public MapPoint Origin { get; set; }
        public MapPoint Destination { get; set; }

        public double Meters { get; set; }
        public double Km { get { return Meters / 1000; } }

        public double Seconds { get; set; }
        public double Minutes { get { return Seconds / 60; } }
        public double Hours { get { return Minutes / 60; } }

        public DateTime DtChegada { get; set; }

        public Route()
        {

        }

        public bool Equals(Route obj)
        {
            return Origin.Equals(obj.Origin) && Destination.Equals(obj.Destination);
        }
    }
}