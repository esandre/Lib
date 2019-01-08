using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lib.Collections.Chained
{
    internal class ChainedEnumerator<TElement> : IEnumerator<TElement>
    {
        private IEnumerator<TElement> _currentChunk;
        private readonly IEnumerator<IEnumerable<TElement>> _chunkEnumerator;

        public ChainedEnumerator(IEnumerator<IEnumerable<TElement>> chunkEnumerator)
        {
            _chunkEnumerator = chunkEnumerator;
        }

        public bool MoveNext() => _currentChunk.MoveNext() || MoveNextChunk();

        public void Reset()
        {
            var currentChunk = _currentChunk;

            _chunkEnumerator.Reset();
            _currentChunk = default(IEnumerator<TElement>);

            currentChunk.Dispose();
        }

        public TElement Current => _currentChunk.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _currentChunk.Dispose();
            _chunkEnumerator.Dispose();
        }

        private bool MoveNextChunk()
        {
            // No more chunks
            if (!_chunkEnumerator.MoveNext()) return false;

            Debug.Assert(_chunkEnumerator.Current != null, "_chunkEnumerator.Current != null");
            var nextChunk = _chunkEnumerator.Current.GetEnumerator();
            
            var previousChunk = _currentChunk;
            _currentChunk = nextChunk;
            previousChunk.Dispose();

            return _currentChunk.MoveNext();
        }
    }
}
