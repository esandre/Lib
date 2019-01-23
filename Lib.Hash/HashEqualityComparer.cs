using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Hash
{
    /// <summary>
    /// Esquality comparer between <see cref="IHash"/>
    /// </summary>
    public class HashEqualityComparer : IEqualityComparer<IHash> 
    {
        /// <inheritdoc />
        public bool Equals(IHash x, IHash y)
        {
            if (x is null) return y is null;
            if (y is null) return false;
            return x.Bytes.SequenceEqual(y.Bytes);
        }

        /// <inheritdoc />
        public int GetHashCode(IHash obj) => BitConverter.ToInt32(obj.Bytes, 0);
    }
}
