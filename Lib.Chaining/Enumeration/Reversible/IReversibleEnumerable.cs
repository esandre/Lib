using System.Collections;
using System.Collections.Generic;

namespace Lib.Chaining.Enumeration.Reversible
{
    /// <summary>
    /// Reversible IEnumerable
    /// </summary>
    public interface IReversibleEnumerable : IEnumerable
    {
        /// <summary>
        ///   Retourne un énumérateur réversible qui itère au sein d'une collection.
        /// </summary>
        /// <returns>
        ///   Objet <see cref="IReversibleEnumerator" /> pouvant être utilisé pour itérer au sein de la collection.
        /// </returns>
        new IReversibleEnumerator GetEnumerator();
    }

    /// <summary>
    /// Reversible IEnumerable
    /// </summary>
    public interface IReversibleEnumerable<out TElement> : IEnumerable<TElement>, IReversibleEnumerable
    {
        /// <summary>
        ///   Retourne un énumérateur réversible qui itère au sein d'une collection.
        /// </summary>
        /// <returns>
        ///   Objet <see cref="IReversibleEnumerator" /> pouvant être utilisé pour itérer au sein de la collection.
        /// </returns>
        new IReversibleEnumerator<TElement> GetEnumerator();
    }
}
