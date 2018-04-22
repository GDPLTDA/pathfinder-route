using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class Extensions
    {
        public static Task WhenAllAsync(this IEnumerable<Task> @this) => Task.WhenAll(@this);
        public static Task<T[]> WhenAllAsync<T>(this IEnumerable<Task<T>> @this) => Task.WhenAll(@this);
    }
}
