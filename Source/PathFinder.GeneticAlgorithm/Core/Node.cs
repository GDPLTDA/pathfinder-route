using PathFinder.Routes;

namespace PathFinder.GeneticAlgorithm
{
    public class Node
    {
        public MapPoint MapPoint { get; set; }

        public Node(Node node)
        {
            var type = node.GetType();
            foreach (var item in type.GetProperties())
                item.SetValue(this, item.GetValue(node));
        }
        public Node(MapPoint mapPoint)
        {
            MapPoint = mapPoint;
        }
    }
}