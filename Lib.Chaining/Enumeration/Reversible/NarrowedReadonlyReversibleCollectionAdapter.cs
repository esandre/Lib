using System;
using System.Collections;
using System.Collections.Generic;

namespace Lib.Chaining.Enumeration.Reversible
{
    internal class NarrowedReadonlyReversibleCollectionAdapter<TPayload> : IReadonlyReversibleCollection<TPayload>
    {
        private readonly Predicate<TPayload> _isLowerBoundary;
        private readonly Predicate<TPayload> _isUpperBoundary;
        private readonly IReversibleEnumerable<TPayload> _decorated;

        public NarrowedReadonlyReversibleCollectionAdapter(
            IReversibleEnumerable<TPayload> decorated, 
            TPayload lowerBoundary, 
            TPayload upperBoundary)
        {
            _decorated = decorated;
            _isLowerBoundary = payload => lowerBoundary.Equals(payload);
            _isUpperBoundary = payload => upperBoundary.Equals(payload);
        }

        public IReversibleEnumerator<TPayload> GetEnumerator() 
            => new ReversibleEnumeratorBoundariesDecorator<TPayload>(
                _decorated.GetEnumerator(), 
                _isLowerBoundary, 
                _isUpperBoundary);

        IEnumerator<TPayload> IEnumerable<TPayload>.GetEnumerator() => GetEnumerator();
        IReversibleEnumerator IReversibleEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count
        {
            get
            {
                var num = 0;
                using (var enumerator = ((IEnumerable<TPayload>)this).GetEnumerator())
                    while (enumerator.MoveNext())
                    {
                        checked { ++num; }
                    }
                return num;
            }
        }
    }
}
