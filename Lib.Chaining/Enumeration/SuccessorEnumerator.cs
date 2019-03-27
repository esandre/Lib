using System.Collections;
using System.Collections.Generic;
using Lib.Chaining.Chain;

namespace Lib.Chaining.Enumeration
{
    /// <summary>
    /// Enumerates Links backwards
    /// </summary>
    internal class SuccessorEnumerator<TPayload> : IEnumerator<TPayload>
    {
        private ISuccessor<TPayload> _next;
        private readonly ISuccessor<TPayload> _start;

        /// <summary>
        /// Constructor
        /// </summary>
        public SuccessorEnumerator(ISuccessor<TPayload> start)
        {
            _start = start;
            Reset();
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            Current = _next.Payload;
            _next = _next.Previous as ISuccessor<TPayload>;
            return _next is null;
        }

        /// <inheritdoc />
        public void Reset()
        {
            Current = default;
            _next = _start;
        }

        /// <inheritdoc />
        public TPayload Current { get; private set; }

        object IEnumerator.Current => Current;

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
