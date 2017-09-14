using PathFinder.GeneticAlgorithm;
using PathFinder.GeneticAlgorithm.Abstraction;
using System;

namespace PathFinder.GeneticAlgorithm.Factories
{
    public class MutateFactory : IFactory<IMutate, MutateEnum>
    {
        public static IMutate GetEMImplementation()
            => new MutateEM();
        public static IMutate GetDIVMImplementation()
            => new MutateDIVM();
        public static IMutate GetDMImplementation()
            => new MutateDM();
        public static IMutate GetIMImplementation()
            => new MutateIM();
        public static IMutate GetIVMImplementation()
            => new MutateIVM();
        public static IMutate GetSMImplementation()
            => new MutateSM();
        public static IMutate GetImplementation(MutateEnum option)
            => Decide(option);

        private static IMutate Decide(MutateEnum option)
        {
            switch (option)
            {
                case MutateEnum.EM:
                    return GetEMImplementation();
                case MutateEnum.DIVM:
                    return GetDIVMImplementation();
                case MutateEnum.DM:
                    return GetDMImplementation();
                case MutateEnum.IM:
                    return GetIMImplementation();
                case MutateEnum.IVM:
                    return GetIVMImplementation();
                case MutateEnum.SM:
                    return GetSMImplementation();
            }
            throw new Exception("No mutate passed");
        }
    }
}