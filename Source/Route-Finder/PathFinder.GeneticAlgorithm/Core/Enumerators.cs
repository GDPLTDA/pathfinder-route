namespace PathFinder
{
    public enum CrossoverEnum
    {
        SubRouteInsertion,
    }
    public enum MutateEnum
    {
        Swap,
        Inversion,
        Insertion,
        Displacement,
    }
    public enum SelectionEnum
    {
        RouletteWheel
    }
    public enum FitnessEnum
    {
        DistanceAndTime
    }
}