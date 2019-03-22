using System.Collections.Generic;
using System.Threading;

namespace Lib.Collections.Chained.Utils
{
    internal class EnumerableAsyncEnumerableAdapter<TElement> : IAsyncEnumerable<TElement>
    {
        private readonly IEnumerable<TElement> _enumerable;

        public EnumerableAsyncEnumerableAdapter(IEnumerable<TElement> enumerable)
        {
            _enumerable = enumerable;
        }

        public IAsyncEnumerator<TElement> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.ThrowIfCancellationRequested();
            return new EnumeratorAsyncEnumeratorAdapter<TElement>(_enumerable.GetEnumerator());
        }
    }
}
