using System.Collections.Async;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.Collections.Chained.Utils
{
    internal class EnumeratorAsyncEnumeratorAdapter<TElement> : IAsyncEnumerator<TElement>
    {
        private readonly IEnumerator<TElement> _wrapped;

        public EnumeratorAsyncEnumeratorAdapter(IEnumerator<TElement> wrapped)
        {
            _wrapped = wrapped;
        }

        public void Dispose() => _wrapped.Dispose();

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(_wrapped.MoveNext());
        }

        public TElement Current => _wrapped.Current;

        object IAsyncEnumerator.Current => Current;
    }
}
