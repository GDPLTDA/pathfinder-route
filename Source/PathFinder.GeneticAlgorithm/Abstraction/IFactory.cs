

namespace Pathfinder.Abstraction
{
    public interface IFactory<T, TEnum> where TEnum : struct
    {
        T GetImplementation(TEnum option);
    }
    public interface IFactory : IFactory<object, int>
    {
    }

    public interface IFactory<T> : IFactory<T, int>
    {
        T GetImplementation();
    }
}