using System.Collections.Async;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.Collections.Chained.Utils
{
    internal class EnumerableAsyncEnumerableAdapter<TElement> : IAsyncEnumerable<TElement>
    {
        private readonly IEnumerable<TElement> _enumerable;

        public EnumerableAsyncEnumerableAdapter(IEnumerable<TElement> enumerable)
        {
            _enumerable = enumerable;
        }

        public async Task<IAsyncEnumerator<TElement>> GetAsyncEnumeratorAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var enumerator = new EnumeratorAsyncEnumeratorAdapter<TElement>(_enumerable.GetEnumerator());
            return await Task.FromResult((IAsyncEnumerator<TElement>) enumerator);
        }

        async Task<IAsyncEnumerator> IAsyncEnumerable.GetAsyncEnumeratorAsync(CancellationToken cancellationToken)
        {
            return await GetAsyncEnumeratorAsync(cancellationToken);
        }
    }
}
