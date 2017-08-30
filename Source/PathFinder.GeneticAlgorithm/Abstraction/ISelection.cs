using System.Collections.Generic;

namespace Pathfinder.Abstraction
{
    public interface ISelection
    {
        IGenome Select(List<IGenome> listnode);
    }
}