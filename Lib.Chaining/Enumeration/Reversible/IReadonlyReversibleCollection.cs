using System.Collections.Generic;

namespace Lib.Chaining.Enumeration.Reversible
{
    /// <summary>
    /// Couples a <see cref="IReadOnlyCollection{T}"/> and a <see cref="IReversibleEnumerable{TElement}"/>
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    public interface IReadonlyReversibleCollection<out TPayload> : IReversibleEnumerable<TPayload>, IReadOnlyCollection<TPayload>
    {
    }
}
