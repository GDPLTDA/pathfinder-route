namespace PathFinder.GeneticAlgorithm
{
    public class Node
    {
        public Node(Node node)
        {
            var type = node.GetType();
            foreach (var item in type.GetProperties())
                item.SetValue(this, item.GetValue(node));
        }
        public Node(Node node, Node parent)
        {
            var type = node.GetType();
            foreach (var item in type.GetProperties())
            {
                item.SetValue(this, item.GetValue(node));
            }
            ParentNode = parent;
        }
        public Node(int latitude, int longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Node ParentNode { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public double D { get; set; }
        public double T { get; set; }

        public override bool Equals(object _other)
        {
            var other = (Node)_other;
            return Latitude == other?.Latitude && Longitude== other?.Longitude;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(Node node1, Node node2)
        {
            if (ReferenceEquals(node1, null))
            {
                return ReferenceEquals(node2, null);
            }
            return node1.Equals(node2);
        }
        public static bool operator !=(Node node1, Node node2)
        {
            if (ReferenceEquals(node1, null))
            {
                return !ReferenceEquals(node2, null);
            }
            return !node1.Equals(node2);
        }
        public override string ToString()
        {
            return $"{{{this.Latitude},{this.Longitude}";
        }
    }
}