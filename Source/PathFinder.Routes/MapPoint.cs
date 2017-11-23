using System;

namespace PathFinder.Routes
{
    public class MapPoint
    {
        public string Name { get; set; }
        public string Endereco { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Period Period { get; set; }
        public DateTime Date { get; set; }

        public MapPoint()
        {
        }

        public MapPoint(string name, string endereco = null)
        {
            Name = name;
            Endereco = endereco;
            if (endereco == null)
                Endereco = name;
        }

        public override string ToString() => $"({Latitude},{Longitude}) {Name}";

        public bool Equals(MapPoint o) => Latitude == o.Latitude && Longitude == o.Longitude;
    }
}
