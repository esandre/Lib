using System.Collections;
using System.Collections.Generic;
using Lib.Chaining.Structures;

namespace Lib.Chaining.Enumeration
{
    /// <summary>
    /// Enumerates a Segment backwards or forward
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    internal class SegmentReadonlyCollectionAdapter<TPayload> : IReadonlyReversibleCollection<TPayload>
    {
        private readonly ISegment<TPayload> _segment;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="segment"></param>
        public SegmentReadonlyCollectionAdapter(ISegment<TPayload> segment)
        {
            _segment = segment;
        }

        /// <inheritdoc />
        public int Count
        {
            get
            {
                var num = 0;
                using (var enumerator = ((IEnumerable<TPayload>)this).GetEnumerator())
                    while (enumerator.MoveNext()) checked { ++num; }
                return num;
            }
        }

        IEnumerator<TPayload> IEnumerable<TPayload>.GetEnumerator() => new PredecessorEnumerator<TPayload>(_segment.Start);

        IReversibleEnumerator<TPayload> IReversibleEnumerable<TPayload>.GetEnumerator() => new SegmentReversibleEnumerator<TPayload>(_segment);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TPayload>) this).GetEnumerator();

        IReversibleEnumerator IReversibleEnumerable.GetEnumerator() => ((IReversibleEnumerable<TPayload>)this).GetEnumerator();
    }
}
