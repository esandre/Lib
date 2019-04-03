namespace Lib.Chaining.Chain
{
    internal sealed class StartLink<TPayload> : IPredecessor<TPayload>, ISuccessor<TPayload>
    {
        public StartLink(ILink<TPayload> next)
        {
            Next = LinkSuccessorDecorator<TPayload>.Decorate(next, this);
        }

        public StartLink(IPredecessor<TPayload> next)
        {
            Next = LinkSuccessorDecorator<TPayload>.Decorate(next, this);
        }

        public TPayload Payload => default;
        public ISuccessor<TPayload> Next { get; }
        public IPredecessor<TPayload> Previous => this;
    }
}
