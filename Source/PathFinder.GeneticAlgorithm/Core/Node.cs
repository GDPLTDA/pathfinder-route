using PathFinder.Routes;

namespace PathFinder.GeneticAlgorithm
{
    public struct Node
    {
        public MapPoint MapPoint { get; set; }

        public Node(Node node)
        {
            MapPoint = new MapPoint(node.MapPoint.Name);

            MapPoint.Date = node.MapPoint.Date;
            MapPoint.Period = node.MapPoint.Period;
            MapPoint.Latitude = node.MapPoint.Latitude;
            MapPoint.Longitude = node.MapPoint.Longitude;
        }
        public Node(MapPoint mapPoint)
        {
            MapPoint = mapPoint;
        }
    }
}