using System;
using System.Collections.Generic;

namespace Lib.Patterns
{
    /// <summary>
    /// Curryfiable equality comparer
    /// </summary>
    public interface ICurryfiableEqualityComparer<in TCompared> : IEqualityComparer<TCompared>
    {
        /// <summary>
        /// Preprocesses the values of X to speed up comparisons
        /// </summary>
        Func<TCompared, bool> CurryfiedEquals(TCompared x);
    }
}
