namespace Lib.Chaining.Chain
{
    /// <summary>
    /// Wraps a link inside a Predecessor envelope
    /// </summary>
    internal static class LinkPredecessorDecorator<TPayload> 
    {
        public static IPredecessor<TPayload> Decorate(ILink<TPayload> decorated, ISuccessor<TPayload> next)
            => new LinkWrapper(decorated, next);

        public static IDoubleChainedLink<TPayload> Decorate(ISuccessor<TPayload> decorated, ISuccessor<TPayload> next)
            => new SuccessorWrapper(decorated, next);

        private class LinkWrapper : IPredecessor<TPayload>
        {
            private readonly ILink<TPayload> _decorated;

            /// <summary>
            /// Constructor
            /// </summary>
            public LinkWrapper(ILink<TPayload> decorated, ISuccessor<TPayload> next)
            {
                _decorated = decorated;
                Next = next;
            }

            /// <inheritdoc />
            public TPayload Payload => _decorated.Payload;

            /// <inheritdoc />
            public ISuccessor<TPayload> Next { get; }
        }

        private class SuccessorWrapper : IDoubleChainedLink<TPayload>
        {
            private readonly ISuccessor<TPayload> _decorated;

            /// <summary>
            /// Constructor
            /// </summary>
            public SuccessorWrapper(ISuccessor<TPayload> decorated, ISuccessor<TPayload> next)
            {
                _decorated = decorated;
                Next = next;
            }

            /// <inheritdoc />
            public TPayload Payload => _decorated.Payload;

            /// <inheritdoc />
            public ISuccessor<TPayload> Next { get; }

            public IPredecessor<TPayload> Previous => _decorated.Previous;
        }
    }
}
