using System.Collections;
using System.Collections.Generic;
using Lib.Chaining.Structures;

namespace Lib.Chaining.Enumeration.Reversible
{
    /// <summary>
    /// Enumerates a Segment backwards or forward
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    internal class SegmentReadonlyReversibleCollection<TPayload> : IReadonlyReversibleCollection<TPayload>
    {
        private readonly ISegment<TPayload> _segment;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="segment"></param>
        public SegmentReadonlyReversibleCollection(ISegment<TPayload> segment)
        {
            _segment = segment;
        }

        /// <inheritdoc />
        public int Count
        {
            get
            {
                var num = 0;
                var end = _segment.End.Payload;
                using (var enumerator = ((IEnumerable<TPayload>)this).GetEnumerator())
                    while (enumerator.MoveNext())
                    {
                        checked { ++ num; }
                        if (end.Equals(enumerator.Current)) break;
                    }
                return num;
            }
        }

        IEnumerator<TPayload> IEnumerable<TPayload>.GetEnumerator() => new PredecessorEnumerator<TPayload>(_segment.Start);

        IReversibleEnumerator<TPayload> IReversibleEnumerable<TPayload>.GetEnumerator() => new SegmentReversibleEnumerator<TPayload>(_segment);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TPayload>) this).GetEnumerator();

        IReversibleEnumerator IReversibleEnumerable.GetEnumerator() => ((IReversibleEnumerable<TPayload>)this).GetEnumerator();
    }
}
