using System.Collections.Generic;

namespace Pathfinder.Abstraction
{
    public interface IGenome
    {
        IMap Map { get; set; }
        List<Node> ListNodes { get; set; }
        double Fitness { get; set; }
        bool IsEqual(IGenome genome);
    }
}