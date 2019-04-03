namespace Lib.Chaining.Chain
{
    internal sealed class EndLink<TPayload> : ISuccessor<TPayload>, IPredecessor<TPayload>
    {
        public EndLink(ILink<TPayload> previous)
        {
            Previous = LinkPredecessorDecorator<TPayload>.Decorate(previous, this);
        }

        public EndLink(ISuccessor<TPayload> previous)
        {
            Previous = LinkPredecessorDecorator<TPayload>.Decorate(previous, this);
        }

        public TPayload Payload => default;
        public IPredecessor<TPayload> Previous { get; }
        public ISuccessor<TPayload> Next => this;
    }
}
