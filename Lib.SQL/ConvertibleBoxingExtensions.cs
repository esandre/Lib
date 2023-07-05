using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Lib.SQL
{
    public static class ConvertibleBoxingExtensions
    {
        public static IEnumerable<KeyValuePair<TKey, object>> Box<TKey>(this IEnumerable<KeyValuePair<TKey, IConvertible>> input)
            => input?.ToDictionary(kv => kv.Key, kv => (object) kv.Value);

        public static IReadOnlyDictionary<TKey, IConvertible> Unbox<TKey>(this IEnumerable<KeyValuePair<TKey, object>> input)
            => input?.ToDictionary(kv => kv.Key, kv => kv.Value.AsConvertible());

        public static IReadOnlyList<IReadOnlyDictionary<TKey, IConvertible>> Unbox<TKey>(this IEnumerable<IEnumerable<KeyValuePair<TKey, object>>> input)
            => input?.Select(dict => dict.Unbox()).ToList();

        [CanBeNull] public static IConvertible AsConvertible(this object obj) => obj is null || obj is DBNull ? null 
            : obj as IConvertible ?? 
              throw new ArrayTypeMismatchException($"Not IConvertible returned by Db : {obj} of type {obj.GetType()}");
    }
}
