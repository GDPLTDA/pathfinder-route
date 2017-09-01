using PathFinder.GeneticAlgorithm.Abstraction;
using System.Collections.Generic;

namespace PathFinder.GeneticAlgorithm
{
    public class Genome : IGenome
    {
        public List<Node> ListNodes { get; set; }
        public double Fitness { get; set; }
        public Genome()
        {
        }
        public Genome(IGenome genome)
        {
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