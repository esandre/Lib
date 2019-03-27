using System.Collections;
using System.Collections.Generic;

namespace Lib.Chaining.Enumeration
{
    /// <summary>
    /// Enumerator with reversing capability
    /// </summary>
    public interface IReversibleEnumerator : IEnumerator
    {
        /// <summary>
        ///   Avance l’énumérateur à l’élément précédent de la collection.
        /// </summary>
        /// <returns>
        ///   <see langword="true" /> si l'énumérateur a pu avancer jusqu'à l'élément précédent ; <see langword="false" /> si l'énumérateur a dépassé la fin de la collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">
        ///   La collection a été modifiée après la création de l’énumérateur.
        /// </exception>
        bool MovePrevious();
    }

    /// <summary>
    /// Generic Enumerator with reversing capability
    /// </summary>
    public interface IReversibleEnumerator<out TElement> : IEnumerator<TElement>, IReversibleEnumerator
    {
    }
}
