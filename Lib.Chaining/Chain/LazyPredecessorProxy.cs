using System;

namespace Lib.Chaining.Chain
{
    /// <inheritdoc />
    internal class LazyPredecessorProxy<TPayload> : IPredecessor<TPayload>
    {
        private readonly Lazy<IPredecessor<TPayload>> _lazy;

        public LazyPredecessorProxy(Func<IPredecessor<TPayload>> lazy)
        {
            _lazy = new Lazy<IPredecessor<TPayload>>(lazy);
        }

        /// <inheritdoc />
        public TPayload Payload => _lazy.Value.Payload;

        /// <inheritdoc />
        public ISuccessor<TPayload> Next => _lazy.Value.Next;
    }
}
