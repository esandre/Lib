using System;

namespace Lib.Chaining.Chain
{
    /// <inheritdoc />
    internal class LazySuccessorProxy<TPayload> : ISuccessor<TPayload>
    {
        private readonly Lazy<ISuccessor<TPayload>> _lazy;

        public LazySuccessorProxy(Func<ISuccessor<TPayload>> lazy)
        {
            _lazy = new Lazy<ISuccessor<TPayload>>(lazy);
        }

        /// <inheritdoc />
        public TPayload Payload => _lazy.Value.Payload;

        /// <inheritdoc />
        public IPredecessor<TPayload> Previous => _lazy.Value.Previous;
    }
}
