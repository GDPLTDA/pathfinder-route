using System.Collections.Generic;

namespace PathFinder.Routes
{
    public class RouteMap
    {
        public MapPoint Storage { get; set; }
        public List<MapPoint> Destinations { get; set; } = new List<MapPoint>();

        public RouteMap(string name)
        {
            Storage = new MapPoint(name, new GoogleMapas.Location());
        }
        public void AddDestinations(params string[] param)
        {
            foreach (var item in param)
                AddDestination(item);
        }

        public void AddDestination(string destination)
        {
            Destinations.Add(new MapPoint(destination, new GoogleMapas.Location()));
        }
        public void AddDestination(MapPoint destination)
        {
            Destinations.Add(destination);
        }
    }
}
