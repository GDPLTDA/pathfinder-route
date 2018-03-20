namespace VRP.GeneticAlgorithm.Delegates
{
    public delegate (Genome mon, Genome dad) CrossOverDelegate(double crossOverRate, Genome mon, Genome dad);
}
