using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class Extensions
    {
        public static Task WhenAllAsync(this IEnumerable<Task> @this) => Task.WhenAll(@this);
        public static Task<T[]> WhenAllAsync<T>(this IEnumerable<Task<T>> @this) => Task.WhenAll(@this);
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list) => list.Shuffle(new Random());
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list, Random r) =>
                from value in list
                orderby r.Next()
                select value;

        

    }
}
