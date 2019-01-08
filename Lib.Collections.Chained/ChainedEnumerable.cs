using System.Collections;
using System.Collections.Generic;

namespace Lib.Collections.Chained
{
    /// <summary>
    /// An enumerable formed with a chain of chunks
    /// </summary>
    public class ChainedEnumerable<TElement> : IEnumerable<TElement>
    {
        private readonly IEnumerable<IEnumerable<TElement>> _chunkGenerator;

        /// <summary>
        /// Creates an enumerable from a generator of chunks
        /// </summary>
        public ChainedEnumerable(IEnumerable<IEnumerable<TElement>> chunkGenerator)
        {
            _chunkGenerator = chunkGenerator;
        }

        /// <inheritdoc />
        public IEnumerator<TElement> GetEnumerator() => new ChainedEnumerator<TElement>(_chunkGenerator.GetEnumerator());

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
