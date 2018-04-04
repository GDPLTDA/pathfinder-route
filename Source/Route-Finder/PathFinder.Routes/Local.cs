namespace PathFinder.Routes
{
    public class Local
    {
        public string Name { get; set; }
        public string Endereco { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Period Period { get; set; }

        public Local()
        {
        }

        public Local(string name, string endereco = null)
        {
            Name = name;
            Endereco = endereco;
            if (endereco == null)
                Endereco = name;
        }

        public override string ToString() => $"({Latitude},{Longitude}) {Name}";

        public bool Equals(Local o) => Latitude == o.Latitude && Longitude == o.Longitude;
    }
}
