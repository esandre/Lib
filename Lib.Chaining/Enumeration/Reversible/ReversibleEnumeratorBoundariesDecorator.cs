using System;
using System.Collections;

namespace Lib.Chaining.Enumeration.Reversible
{
    internal class ReversibleEnumeratorBoundariesDecorator<TPayload> : IReversibleEnumerator<TPayload>
    {
        private readonly IReversibleEnumerator<TPayload> _unbounded;
        private readonly Predicate<TPayload> _isUpperBoundary;
        private readonly Predicate<TPayload> _isLowerBoundary;
        private bool _isBeyondUpperBoundary;
        private bool _isBeyondLowerBoundary;
        private bool _isInitialized;

        public ReversibleEnumeratorBoundariesDecorator(
            IReversibleEnumerator<TPayload> unbounded,
            Predicate<TPayload> isLowerBoundary,
            Predicate<TPayload> isUpperBoundary)
        {
            _unbounded = unbounded;
            _isUpperBoundary = isUpperBoundary;
            _isLowerBoundary = isLowerBoundary;
            Current = default;
        }

        public bool MoveNext()
        {
            if (_isBeyondUpperBoundary) return false;
            _isBeyondLowerBoundary = false;

            if (_isUpperBoundary(_unbounded.Current) && _isInitialized)
            {
                _unbounded.MoveNext();
                Current = default;
                _isBeyondUpperBoundary = true;
                return false;
            }

            _isInitialized = true;

            var moveNext = _unbounded.MoveNext();
            Current = _unbounded.Current;
            return moveNext;
        }

        public bool MovePrevious()
        {
            if (_isBeyondLowerBoundary) return false;
            _isBeyondUpperBoundary = false;

            if (_isLowerBoundary(_unbounded.Current) && _isInitialized)
            {
                _unbounded.MovePrevious();
                Current = default;
                _isBeyondLowerBoundary = true;
                return false;
            }

            _isInitialized = true;

            var movePrevious = _unbounded.MovePrevious();
            Current = _unbounded.Current;
            return movePrevious;
        }

        public TPayload Current { get; private set; }
        object IEnumerator.Current => Current;
        public void Dispose() => _unbounded.Dispose();
        public void Reset()
        {
            _unbounded.Reset();
            Current = default;
        }
    }
}
