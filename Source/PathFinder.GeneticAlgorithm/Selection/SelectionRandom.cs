using Pathfinder.Abstraction;
using Pathfinder.Factories;
using System.Collections.Generic;
namespace Pathfinder.Selection
{
    public class SelectionRandom : ISelection
    {
        public IGenome Select(List<IGenome> listnode)
        {
            var rand = RandomFactory.Rand;
            var ind = rand.Next(0, listnode.Count);
            return listnode[ind];
        }
    }
}