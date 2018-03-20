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
            Origin = origin;
            Target = target;
            Meters = meters;
            Seconds = seconds;
        }

        public Route(Local origin, Local target)
        {
            Origin = origin;
            Target = target;
            Meters = Seconds = double.MaxValue;
        }

        public bool Equals(Route obj) => Origin.Equals(obj.Origin) && Target.Equals(obj.Target);
    }
}