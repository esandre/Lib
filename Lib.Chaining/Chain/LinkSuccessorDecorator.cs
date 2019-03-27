namespace Lib.Chaining.Chain
{
    /// <summary>
    /// Wraps a link inside a Successor envelope
    /// </summary>
    internal static class LinkSuccessorDecorator<TPayload>
    {
        public static ISuccessor<TPayload> Decorate(ILink<TPayload> decorated, IPredecessor<TPayload> previous) 
            => new LinkWrapper(decorated, previous);

        public static IDoubleChainedLink<TPayload> Decorate(IPredecessor<TPayload> decorated, IPredecessor<TPayload> previous)
            => new PredecessorWrapper(decorated, previous);

        private class LinkWrapper : ISuccessor<TPayload>
        {
            private readonly ILink<TPayload> _decorated;

            /// <summary>
            /// Constructor
            /// </summary>
            public LinkWrapper(ILink<TPayload> decorated, IPredecessor<TPayload> previous)
            {
                _decorated = decorated;
                Previous = previous;
            }

            /// <inheritdoc />
            public TPayload Payload => _decorated.Payload;

            /// <inheritdoc />
            public IPredecessor<TPayload> Previous { get; }
        }

        private class PredecessorWrapper : IDoubleChainedLink<TPayload>
        {
            private readonly IPredecessor<TPayload> _decorated;

            /// <summary>
            /// Constructor
            /// </summary>
            public PredecessorWrapper(IPredecessor<TPayload> decorated, IPredecessor<TPayload> previous)
            {
                _decorated = decorated;
                Previous = previous;
            }

            /// <inheritdoc />
            public TPayload Payload => _decorated.Payload;

            /// <inheritdoc />
            public IPredecessor<TPayload> Previous { get; }

            public ISuccessor<TPayload> Next => _decorated.Next;
        }
    }
}
