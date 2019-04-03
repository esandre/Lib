using System;
using Lib.Chaining.Chain;

namespace Lib.Chaining.Enumeration
{
    internal class PrimerLink<TPayload> : ISuccessor<TPayload>, IPredecessor<TPayload>
    {
        public PrimerLink(ILink<TPayload> firstValue)
        {
            var predecessor = firstValue as IPredecessor<TPayload>;
            var isPredecessor = predecessor != null;

            var successor = firstValue as ISuccessor<TPayload>;
            var isSuccessor = successor != null;

            if (isPredecessor && isSuccessor)
            {
                Previous = predecessor;
                Next = successor;
            }
            else if (isSuccessor)
            {
                var endLink = new EndLink<TPayload>(new LazySuccessorProxy<TPayload>(() => Next));
                var primer = LinkPredecessorDecorator<TPayload>.Decorate(successor, endLink);

                Previous = primer;
                Next = primer;
            }
            else if (isPredecessor)
            {
                var startLink = new StartLink<TPayload>(new LazyPredecessorProxy<TPayload>(() => Previous));
                var primer = LinkSuccessorDecorator<TPayload>.Decorate(predecessor, startLink);

                Previous = primer;
                Next = primer;
            }
            else throw new ArgumentException("Link is neither successor nor predecesor");
        }

        public TPayload Payload => default;
        public IPredecessor<TPayload> Previous { get; }
        public ISuccessor<TPayload> Next { get; }
    }
}
