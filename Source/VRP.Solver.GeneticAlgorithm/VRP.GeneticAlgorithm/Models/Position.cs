namespace VRP.GeneticAlgorithm.Models
{
    public readonly struct Position
    {
        public readonly double Lat;
        public readonly double Long;

        public Position(double lat, double @long)
        {
            Lat = lat;
            Long = @long;
        }

    }
}
