using System;

namespace PathFinder.Routes
{
    public class Local
    {
        public string Name { get; set; }
        public string Endereco { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Period Period { get; set; }

        public Local()
        {
        }

        public Local(string name, string endereco = null)
        {
            Name = name;
            Endereco = endereco;
            if (endereco == null)
                Endereco = name;
        }

        public override string ToString() => $"({Latitude},{Longitude}) {Name}";

        public bool Equals(Local o) => Latitude == o.Latitude && Longitude == o.Longitude;


        public double GetDistanceTo(Local other)
        {
            if (double.IsNaN(this.Latitude) || double.IsNaN(this.Longitude) || double.IsNaN(other.Latitude) || double.IsNaN(other.Longitude))
                throw new ArgumentException("Invalid arguments");

            var latitude = this.Latitude * 0.0174532925199433;
            var longitude = this.Longitude * 0.0174532925199433;
            var num = other.Latitude * 0.0174532925199433;
            var longitude1 = other.Longitude * 0.0174532925199433;
            var num1 = longitude1 - longitude;
            var num2 = num - latitude;
            var num3 = Math.Pow(Math.Sin(num2 / 2), 2) + Math.Cos(latitude) * Math.Cos(num) * Math.Pow(Math.Sin(num1 / 2), 2);
            var num4 = 2 * Math.Atan2(Math.Sqrt(num3), Math.Sqrt(1 - num3));
            var num5 = 6376500 * num4;
            return num5;
        }

    }
}
