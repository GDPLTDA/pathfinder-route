using System;

namespace VRP.GeneticAlgorithm.Models
{
    public class Route
    {
        public Local Origin { get; }
        public Local Target { get; }
        public double Meters { get; }
        public double Seconds { get; }

        public Route(Local origin, Local target, double meters, double seconds)
        {
            Origin = origin ?? throw new ArgumentNullException(nameof(origin));
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Meters = meters;
            Seconds = seconds;
        }

        public Route(Local origin, Local target) : this(origin, target, double.MaxValue, double.MaxValue) { }

        public bool Equals(Route obj) => Origin.Equals(obj.Origin) && Target.Equals(obj.Target);

        public override string ToString() => $"{Origin.Name} => {Target.Name} ({Meters}m)";
    }
}