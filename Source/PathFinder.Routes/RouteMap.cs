using System;
using System.Collections.Generic;

namespace PathFinder.Routes
{
    public class RouteMap
    {
        public MapPoint Storage { get; set; }
        public List<MapPoint> Destinations { get; set; } = new List<MapPoint>();

        public RouteMap(string name, DateTime? now = null)
        {
            Load(name, now);
        }
        async void Load(string name, DateTime? now)
        {
            Storage = await SearchRoute.GetPointAsync(new MapPoint(name));

            if (now == null)
                now = DateTime.Now;

            Storage.Date = (DateTime)now;
        }
        public void AddDestinations(params string[] param)
        {
            foreach (var item in param)
                AddDestination(item);
        }
        public async void AddDestination(string destination)
        {
            var point = await SearchRoute.GetPointAsync(new MapPoint(destination));
            Destinations.Add(point);
        }
    }
}
