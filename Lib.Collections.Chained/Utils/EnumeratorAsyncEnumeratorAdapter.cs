using System.Collections.Generic;
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

        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(Task.FromResult(_wrapped.MoveNext()));

        public TElement Current => _wrapped.Current;

        public ValueTask DisposeAsync()
        {
            _wrapped.Dispose();
            return new ValueTask();
        }
    }
}
