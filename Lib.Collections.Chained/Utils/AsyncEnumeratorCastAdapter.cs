using System;
using System.Collections.Generic;
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

        public async ValueTask<bool> MoveNextAsync()
        {
            var sourceMoveNext = await _source.MoveNextAsync();
            _current = new Lazy<TDestination>(() => _castFunc(_source.Current));
            return sourceMoveNext;
        }

        public TDestination Current => _current.Value;

        public async ValueTask DisposeAsync()
        {
            await _source.DisposeAsync();
        }
    }
}
