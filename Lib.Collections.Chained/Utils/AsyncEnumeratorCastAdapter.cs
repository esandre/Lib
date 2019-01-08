using System;
using System.Collections.Async;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.Collections.Chained.Utils
{
    internal class AsyncEnumeratorCastAdapter<TSource, TDestination> : IAsyncEnumerator<TDestination>
    {
        private readonly IAsyncEnumerator<TSource> _source;
        private readonly Func<TSource, TDestination> _castFunc;
        private Lazy<TDestination> _current;

        public AsyncEnumeratorCastAdapter(IAsyncEnumerator<TSource> source, Func<TSource, TDestination> castFunc)
        {
            _source = source;
            _castFunc = castFunc;
        }

        public void Dispose()
        {
            _source.Dispose();
        }

        public async Task<bool> MoveNextAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var sourceMoveNext = await _source.MoveNextAsync(cancellationToken);
            _current = new Lazy<TDestination>(() => _castFunc(_source.Current));
            return sourceMoveNext;
        }

        public TDestination Current => _current.Value;

        object IAsyncEnumerator.Current => Current;
    }
}
