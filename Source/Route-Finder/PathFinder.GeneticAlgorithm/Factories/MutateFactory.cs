using PathFinder.GeneticAlgorithm.Abstraction;
using System;

namespace PathFinder.GeneticAlgorithm.Factories
{
    public class MutateFactory
    {
        public static IMutate GetEMImplementation(GASettings settings) => new MutateEM(settings);
        public static IMutate GetDIVMImplementation(GASettings settings) => new MutateDIVM(settings);
        public static IMutate GetDMImplementation(GASettings setting) => new MutateDM(setting);
        public static IMutate GetIMImplementation(GASettings setting) => new MutateIM(setting);
        public static IMutate GetIVMImplementation(GASettings setting) => new MutateIVM(setting);
        public static IMutate GetSMImplementation(GASettings setting) => new MutateSM(setting);
        public static IMutate GetImplementation(MutateEnum option, GASettings settings) => Decide(option, settings);

        private static IMutate Decide(MutateEnum option, GASettings settings)
        {
            switch (option)
            {
                case MutateEnum.EM:
                    return GetEMImplementation(settings);
                case MutateEnum.DIVM:
                    return GetDIVMImplementation(settings);
                case MutateEnum.DM:
                    return GetDMImplementation(settings);
                case MutateEnum.IM:
                    return GetIMImplementation(settings);
                case MutateEnum.IVM:
                    return GetIVMImplementation(settings);
                case MutateEnum.SM:
                    return GetSMImplementation(settings);
            }
            throw new Exception("No mutate passed");
        }
    }
}