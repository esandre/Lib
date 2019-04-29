using System.Collections;
using Lib.Chaining.Structures;

namespace Lib.Chaining.Enumeration.Reversible
{
    /// <summary>
    /// Enumerates a Segment from either the Start or the End depending of the direction chosen initially
    /// </summary>
    internal class SegmentReversibleEnumerator<TPayload> : IReversibleEnumerator<TPayload>
    {
        private readonly ISegment<TPayload> _segment;
        private IReversibleEnumerator<TPayload> _inner;

        /// <summary>
        /// Constructor
        /// </summary>
        public SegmentReversibleEnumerator(ISegment<TPayload> segment)
        {
            _segment = segment;
        }

        private bool IsLowerBoundary(TPayload payload) => _segment.Start.Payload.Equals(payload);
        private bool IsUpperBoundary(TPayload payload) => _segment.End.Payload.Equals(payload);

        /// <inheritdoc />
        public bool MoveNext()
        {
            if (_inner is null) _inner = new ReversibleEnumeratorBoundariesDecorator<TPayload>(
                new LinkReversibleEnumerator<TPayload>(_segment.Start),
                IsLowerBoundary, 
                IsUpperBoundary);

            return _inner.MoveNext();
        }

        /// <inheritdoc />
        public bool MovePrevious()
        {
            if (_inner is null) _inner = new ReversibleEnumeratorBoundariesDecorator<TPayload>(
                new LinkReversibleEnumerator<TPayload>(_segment.End),
                IsLowerBoundary,
                IsUpperBoundary);

            return _inner.MovePrevious();
        }

        /// <inheritdoc />
        public void Reset()
        {
            _inner?.Dispose();
            _inner = null;
        }

        /// <inheritdoc />
        public TPayload Current => _inner is null ? default : _inner.Current;

        object IEnumerator.Current => Current;

        /// <inheritdoc />
        public void Dispose()
        {
            _inner?.Dispose();
        }
    }
}
