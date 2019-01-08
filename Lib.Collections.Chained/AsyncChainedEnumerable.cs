using System;
using System.Collections.Async;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.Collections.Chained
{
    /// <summary>
    /// An async enumerable formed with a chain of chunks. The chain, the chunks or both must be async.
    /// Otherwise use <see cref="ChainedEnumerable{TElement}"/>
    /// </summary>
    public class AsyncChainedEnumerable<TElement> : IAsyncEnumerable<TElement>
    {
        private readonly Func<CancellationToken, Task<IAsyncEnumerator<TElement>>> _asyncEnumeratorGenerator;

        /// <summary>
        /// Builds from an async enumerable of sync chunks
        /// </summary>
        public AsyncChainedEnumerable(IAsyncEnumerable<IEnumerable<TElement>> chunkGenerator)
        {
            _asyncEnumeratorGenerator = async cancellationToken
                => new AsyncChainedEnumerator<TElement>(await chunkGenerator.GetAsyncEnumeratorAsync(cancellationToken));
        }

        /// <summary>
        /// Builds from a sync enumerable of async chunks
        /// </summary>
        public AsyncChainedEnumerable(IEnumerable<IAsyncEnumerable<TElement>> chunkGenerator)
        {
            _asyncEnumeratorGenerator = async _ 
                => await Task.FromResult(new AsyncChainedEnumerator<TElement>(chunkGenerator.GetEnumerator()));
        }

        /// <summary>
        /// Builds from an async enumerable of async chunks
        /// </summary>
        public AsyncChainedEnumerable(IAsyncEnumerable<IAsyncEnumerable<TElement>> chunkGenerator)
        {
            _asyncEnumeratorGenerator = async cancellationToken
                => new AsyncChainedEnumerator<TElement>(await chunkGenerator.GetAsyncEnumeratorAsync(cancellationToken));
        }

        /// <inheritdoc />
        public async Task<IAsyncEnumerator<TElement>> GetAsyncEnumeratorAsync(CancellationToken cancellationToken)
            => await _asyncEnumeratorGenerator(cancellationToken);

        async Task<IAsyncEnumerator> IAsyncEnumerable.GetAsyncEnumeratorAsync(CancellationToken cancellationToken) 
            => await GetAsyncEnumeratorAsync(cancellationToken);
    }
}
