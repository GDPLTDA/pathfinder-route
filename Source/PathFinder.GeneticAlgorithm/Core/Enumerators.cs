namespace PathFinder
{
    public enum CrossoverEnum
    {
        Simple,
        OBX,
        PBX
    }
    public enum MutateEnum
    {
        EM,
        DIVM,
        DM,
        IM,
        IVM,
        SM,
        Bitwise
    }
    public enum SelectionEnum
    {
        RouletteWheel
    }
    public enum FitnessEnum
    {
        TimePath
    }
}