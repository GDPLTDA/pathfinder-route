using PathFinder.Routes;

namespace PathFinder.GeneticAlgorithm
{
    public class Node
    {
        public MapPoint MapPoint { get; set; }
        //public MapPoint MapPointFinal { get; set; }
        //public double D { get; set; }
        //public double T { get; set; }

        public Node(Node node)
        {
            var type = node.GetType();
            foreach (var item in type.GetProperties())
                item.SetValue(this, item.GetValue(node));
        }
        //public Node(Node node, Node parent)
        //{
        //    var type = node.GetType();
        //    foreach (var item in type.GetProperties())
        //    {
        //        item.SetValue(this, item.GetValue(node));
        //    }
        //    ParentNode = parent;
        //}
        public Node(MapPoint mapPoint)
        {
            MapPoint = mapPoint;
        }
        ////public override bool Equals(object _other)
        ////{
        ////    var other = (Node)_other;
        ////    return MapPoint.Latitude == other?.MapPoint.Latitude && MapPoint.Longitude == other?.MapPoint.Longitude;
        ////}
        ////public override int GetHashCode()
        ////{
        ////    return base.GetHashCode();
        ////}
        ////public static bool operator ==(Node node1, Node node2)
        ////{
        ////    if (ReferenceEquals(node1, null))
        ////    {
        ////        return ReferenceEquals(node2, null);
        ////    }
        ////    return node1.Equals(node2);
        ////}
        ////public static bool operator !=(Node node1, Node node2)
        ////{
        ////    if (ReferenceEquals(node1, null))
        ////    {
        ////        return !ReferenceEquals(node2, null);
        ////    }
        ////    return !node1.Equals(node2);
        ////}
        ////public override string ToString()
        ////{
        ////    return $"{{{MapPoint.Latitude},{MapPoint.Longitude}";
        ////}
    }
}