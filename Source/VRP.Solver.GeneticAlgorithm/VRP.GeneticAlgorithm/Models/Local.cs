namespace VRP.GeneticAlgorithm.Models
{
    public class Local
    {
        public string Name { get; }
        public string Address { get; }
        public Position Position { get; }
        public Period Period { get; }

        public Local(string name, string address, Position position, Period period)
        {
            Name = name;
            Address = address;
            Position = position;
            Period = period;
        }

        public override string ToString() => $"({Position.Lat},{Position.Long}) {Name}";

        public bool Equals(Local o) => Position.Lat == o.Position.Lat
                                    && Position.Long == o.Position.Long;
    }
}
