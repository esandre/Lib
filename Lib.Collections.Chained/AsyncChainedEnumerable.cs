using System;
using System.Collections.Generic;
using System.Threading;

namespace Lib.Collections.Chained
{
    /// <summary>
    /// An async enumerable formed with a chain of chunks. The chain, the chunks or both must be async.
    /// Otherwise use <see cref="ChainedEnumerable{TElement}"/>
    /// </summary>
    public class AsyncChainedEnumerable<TElement> : IAsyncEnumerable<TElement>
    {
        private readonly Func<CancellationToken, IAsyncEnumerator<TElement>> _asyncEnumeratorGenerator;

        /// <summary>
        /// Builds from an async enumerable of sync chunks
        /// </summary>
        public AsyncChainedEnumerable(IAsyncEnumerable<IEnumerable<TElement>> chunkGenerator)
        {
            _asyncEnumeratorGenerator = cancellationToken => new AsyncChainedEnumerator<TElement>(chunkGenerator.GetAsyncEnumerator(), cancellationToken);
        }

        /// <summary>
        /// Builds from a sync enumerable of async chunks
        /// </summary>
        public AsyncChainedEnumerable(IEnumerable<IAsyncEnumerable<TElement>> chunkGenerator)
        {
            _asyncEnumeratorGenerator = cancellationToken => new AsyncChainedEnumerator<TElement>(chunkGenerator.GetEnumerator(), cancellationToken);
        }

        /// <summary>
        /// Builds from an async enumerable of async chunks
        /// </summary>
        public AsyncChainedEnumerable(IAsyncEnumerable<IAsyncEnumerable<TElement>> chunkGenerator)
        {
            _asyncEnumeratorGenerator = cancellationToken => new AsyncChainedEnumerator<TElement>(chunkGenerator.GetAsyncEnumerator(), cancellationToken);
        }

        /// <inheritdoc />
        public IAsyncEnumerator<TElement> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken()) => _asyncEnumeratorGenerator(cancellationToken);
    }
}
