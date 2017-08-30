using System;
namespace PathFinder.Routes
{
    public class Route
    {
		public bool Sucess { get; set; }
		public string Origin { get; set; }
		public string Destination { get; set; }

		public double Meters { get; set; }
        public double Km { get { return Meters / 1000; } }

		public double Seconds { get; set; }
        public double Minutes { get { return Seconds / 60; } }
		public double Hours { get { return Minutes / 60; } }

        public Route()
        {
            
        }
    }
}