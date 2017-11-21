namespace PathFinder.Routes
{
    public class Location
    {
        public Location(double lat, double lon)
        {
            Latitude = lat;
            Longitude = lon;
        }

        public double Latitude { get; }
        public double Longitude { get; }
    }
}
