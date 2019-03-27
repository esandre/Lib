using System.Collections;
using System.Collections.Generic;
using Lib.Chaining.Chain;

namespace Lib.Chaining.Enumeration
{
    /// <summary>
    /// Enumerates Links forward
    /// </summary>
    internal class PredecessorEnumerator<TPayload> : IEnumerator<TPayload>
    {
        private IPredecessor<TPayload> _current;
        private readonly IPredecessor<TPayload> _start;

        /// <summary>
        /// Constructor
        /// </summary>
        public PredecessorEnumerator(IPredecessor<TPayload> start)
        {
            _start = start;
            Reset();
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            var next = _current.Next;

            _current = next is IPredecessor<TPayload> predecessor 
                ? predecessor 
                : LinkPredecessorDecorator<TPayload>.Decorate(next, new EndLink<TPayload>(next));

            return !(next is EndLink<TPayload>);
        }

        /// <inheritdoc />
        public void Reset()
        {
            _current = new StartLink<TPayload>(_start);
        }

        /// <inheritdoc />
        public TPayload Current => _current.Payload;

        object IEnumerator.Current => Current;

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
