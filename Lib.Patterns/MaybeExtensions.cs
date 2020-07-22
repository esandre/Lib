using System.Collections.Generic;
using System.Linq;

namespace Lib.Patterns
{
    public static class MaybeExtensions
    {
        public static IEnumerable<T> SelectThoseWithElements<T>(this IEnumerable<Existe<T>> exists)
        {
            return exists.Where(m => m.IsPresent)
                .Select(existe => existe.SelectIfPresentOrThrow());
        }
    }
}
