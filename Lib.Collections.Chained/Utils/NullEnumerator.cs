using System;
using System.Collections;
using System.Collections.Generic;

namespace Lib.Collections.Chained.Utils
{
    /// <summary>
    /// NullObject pattern for IEnumerator
    /// </summary>
    internal class NullEnumerator : IEnumerator
    {
        public bool MoveNext() => false;

        public void Reset() {}

        public object Current => throw new InvalidOperationException("This is a null Enumerator");
    }

    internal class NullEnumerator<TElement> : NullEnumerator, IEnumerator<TElement>
    {
        TElement IEnumerator<TElement>.Current => throw new InvalidOperationException("This is a null Enumerator");

        public void Dispose() {}
    }
}
