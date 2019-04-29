using System.Collections;
using System.Collections.Generic;
using Lib.Chaining.Chain;

namespace Lib.Chaining.Enumeration.Reversible
{
    internal class LinkReversibleEnumerable<TPayload> : IReversibleEnumerable<TPayload>
    {
        private readonly ILink<TPayload> _primer;

        public LinkReversibleEnumerable(ILink<TPayload> primer)
        {
            _primer = primer;
        }

        public IReversibleEnumerator<TPayload> GetEnumerator() => new LinkReversibleEnumerator<TPayload>(_primer);
        IEnumerator<TPayload> IEnumerable<TPayload>.GetEnumerator() => GetEnumerator();
        IReversibleEnumerator IReversibleEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
