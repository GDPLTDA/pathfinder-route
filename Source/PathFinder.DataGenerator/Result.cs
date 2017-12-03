using System.Linq;

namespace PathFinder.DataGenerator
{
    public class Result
    {
        public TipoErro TipoErro { get; set; }
        public string FileName { get; set; }
        public int QtdEntregadores { get; set; }
        public MutateEnum Mutation { get; set; }
        public CrossoverEnum CrossoverEnum { get; set; }
        public double Fitness { get; set; }

        public Result(
            TipoErro TipoErro,
            string FileName,
            int QtdEntregadores,
            MutateEnum Mutation,
            CrossoverEnum CrossoverEnum,
            double Fitness
          )
        {
            this.TipoErro = TipoErro;
            this.FileName = FileName;
            this.QtdEntregadores = QtdEntregadores;
            this.Mutation = Mutation;
            this.CrossoverEnum = CrossoverEnum;
            this.Fitness = Fitness;
        }

        public override string ToString() => GetType()
                                                .GetProperties().ToList()
                                                .Select(p => p.GetValue(this)
                                                ?.ToString() ?? string.Empty)
                                                .Aggregate((a, b) => $"{a};{b}")
                                                + "\n";

    }
}
