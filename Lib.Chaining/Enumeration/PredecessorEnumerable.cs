using System.Collections;
using System.Collections.Generic;
using Lib.Chaining.Chain;

namespace Lib.Chaining.Enumeration
{
    /// <summary>
    /// Enumerates a Link succession forward
    /// </summary>
    internal class PredecessorEnumerable<TPayload> : IEnumerable<TPayload>
    {
        private readonly IPredecessor<TPayload> _primer;

        /// <summary>
        /// Constructor
        /// </summary>
        public PredecessorEnumerable(IPredecessor<TPayload> primer)
        {
            _primer = primer;
        }

        /// <inheritdoc />
        public IEnumerator<TPayload> GetEnumerator() => new PredecessorEnumerator<TPayload>(_primer);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
