using System;

namespace PathFinder.Routes
{
    public class MapPoint
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Period Period { get; set; } = new Period();
        public DateTime Date { get; set; }

        public MapPoint()
        {
        }

        public MapPoint(string name)
        {
            Name = name;
        }

        public override string ToString() => $"({Latitude},{Longitude}) {Name}";

        public bool Equals(MapPoint o) => Latitude == o.Latitude && Longitude == o.Longitude;
    }
}
