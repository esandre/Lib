using System.Collections.Async;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Lib.Collections.Chained.Utils;

namespace Lib.Collections.Chained
{
    internal class AsyncChainedEnumerator<TElement> : IAsyncEnumerator<TElement>
    {
        private IAsyncEnumerator<TElement> _currentChunk;
        private readonly IAsyncEnumerator<IAsyncEnumerable<TElement>> _chunkEnumerator;

        public AsyncChainedEnumerator(IEnumerator<IAsyncEnumerable<TElement>> chunkEnumerator)
        {
            _chunkEnumerator = new EnumeratorAsyncEnumeratorAdapter<IAsyncEnumerable<TElement>>(chunkEnumerator);
        }

        public AsyncChainedEnumerator(IAsyncEnumerator<IEnumerable<TElement>> chunkEnumerator)
        {
            _chunkEnumerator = new AsyncEnumeratorCastAdapter<IEnumerable<TElement>, IAsyncEnumerable<TElement>>(
                chunkEnumerator, 
                enumerable => new EnumerableAsyncEnumerableAdapter<TElement>(enumerable));
        }

        public AsyncChainedEnumerator(IAsyncEnumerator<IAsyncEnumerable<TElement>> chunkEnumerator)
        {
            _chunkEnumerator = chunkEnumerator;
        }

        public void Dispose()
        {
            _currentChunk.Dispose();
            _chunkEnumerator.Dispose();
        }

        public async Task<bool> MoveNextAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            //No more chunks
            if (!await _chunkEnumerator.MoveNextAsync(cancellationToken)) return false;

            Debug.Assert(_chunkEnumerator.Current != null, "_chunkEnumerator.Current != null");
            var nextChunk = await _chunkEnumerator.Current.GetAsyncEnumeratorAsync(cancellationToken);

            var previousChunk = _currentChunk;
            _currentChunk = nextChunk;
            previousChunk.Dispose();

            return await _currentChunk.MoveNextAsync(cancellationToken);
        }

        public TElement Current => _currentChunk.Current;

        object IAsyncEnumerator.Current => Current;
    }
}
