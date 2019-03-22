using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Lib.Collections.Chained.Utils;

namespace Lib.Collections.Chained
{
    internal class AsyncChainedEnumerator<TElement> : IAsyncEnumerator<TElement>
    {
        private readonly CancellationToken _cancellationToken;
        private IAsyncEnumerator<TElement> _currentChunk;
        private readonly IAsyncEnumerator<IAsyncEnumerable<TElement>> _chunkEnumerator;

        public AsyncChainedEnumerator(IEnumerator<IAsyncEnumerable<TElement>> chunkEnumerator, CancellationToken cancellationToken)
        {
            _chunkEnumerator = new EnumeratorAsyncEnumeratorAdapter<IAsyncEnumerable<TElement>>(chunkEnumerator);
            _cancellationToken = cancellationToken;
        }

        public AsyncChainedEnumerator(IAsyncEnumerator<IEnumerable<TElement>> chunkEnumerator, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _chunkEnumerator = new AsyncEnumeratorCastAdapter<IEnumerable<TElement>, IAsyncEnumerable<TElement>>(
                chunkEnumerator, 
                enumerable => new EnumerableAsyncEnumerableAdapter<TElement>(enumerable));
        }

        public AsyncChainedEnumerator(IAsyncEnumerator<IAsyncEnumerable<TElement>> chunkEnumerator, CancellationToken cancellationToken)
        {
            _chunkEnumerator = chunkEnumerator;
            _cancellationToken = cancellationToken;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            _cancellationToken.ThrowIfCancellationRequested();

            //No more chunks
            if (!await _chunkEnumerator.MoveNextAsync()) return false;

            Debug.Assert(_chunkEnumerator.Current != null, "_chunkEnumerator.Current != null");
            var nextChunk = _chunkEnumerator.Current.GetAsyncEnumerator();

            var previousChunk = _currentChunk;
            _currentChunk = nextChunk;
            await previousChunk.DisposeAsync();

            return await _currentChunk.MoveNextAsync();
        }

        public TElement Current => _currentChunk.Current;

        public async ValueTask DisposeAsync()
        {
            await _currentChunk.DisposeAsync();
            await _chunkEnumerator.DisposeAsync();
        }
    }
}
