using Pathfinder.Abstraction;
using Pathfinder.Factories;
using System.Collections.Generic;
namespace Pathfinder
{
    public class Genome : IGenome
    {
        public IMap Map { get; set; }
        public List<Node> ListNodes { get; set; }
        public double Fitness { get; set; }
        public Genome()
        {
        }
        public Genome(IMap map, List<Node> listnode)
        {
            Map = map;
            ListNodes = Copy(listnode);
        }
        public Genome(IGenome genome)
        {
            Map = genome.Map;
            ListNodes = Copy(genome.ListNodes);
        }
        public Genome(IMap map)
        {
            Map = map;
            ListNodes = RouteFinding(map.Diagonal);
        }
        public List<Node> RouteFinding(DiagonalMovement diagonal)
        {
            var rand = RandomFactory.Rand;
            var listnode = new List<Node>();
            var run = true;
            var node = new Node(Map.StartNode);
            while (run)
            {
                if (!listnode.Exists(i => i.EqualsAll(node)))
                    listnode.Add(node);
                var list = Map.GetNeighbors(node, diagonal, false, false);
                var ind = rand.Next(0, list.Count);
                var newnode = list[ind];
                run = newnode != null && !listnode.Exists(i => i.EqualsAll(newnode));
                if (newnode != null)
                    node = new Node(newnode, node, newnode.Direction);
            }
            return listnode;
        }
        public bool IsEqual(IGenome genome)
        {
            if (ListNodes.Count != genome.ListNodes.Count)
                return false;
            for (int i = 0; i < ListNodes.Count; i++)
            {
                if (ListNodes[i] != genome.ListNodes[i])
                    return false;
            }
            return true;
        }
        private static List<Node> Copy(List<Node> listnode)
        {
            var returnnode = new List<Node>();
            foreach (var item in listnode)
                returnnode.Add(new Node(item));
            return returnnode;
        }
        public override string ToString()
        {
            return $"F={Fitness}";
        }
    }
}