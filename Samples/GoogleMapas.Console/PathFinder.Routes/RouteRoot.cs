﻿using System.Collections.Generic;

namespace PathFinder.Route.GoogleMapas
{
    public class RouteRoot
    {
        public List<GeocodedWaypoint> geocoded_waypoints { get; set; }
        public List<Route> routes { get; set; }
        public string status { get; set; }
    }
}
