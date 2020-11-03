using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.Patterns
{
    public class AsyncLazy<T>
    {
        private static Func<Task<T>> DefaultValue => () => Task.FromResult(new Lazy<T>().Value);
        private static Func<Task<T>> FromFuncT(Func<T> valueFunc) => () => Task.FromResult(valueFunc());
        private readonly Lazy<Task<T>> _innerLazy;

        public AsyncLazy()
        {
            _innerLazy = new Lazy<Task<T>>(DefaultValue);
        }

        public AsyncLazy(T value)
        {
            _innerLazy = new Lazy<Task<T>>(Task.FromResult(value));
        }

        public AsyncLazy(bool isThreadSafe)
        {
            _innerLazy = new Lazy<Task<T>>(DefaultValue, isThreadSafe);
        }

        public AsyncLazy(LazyThreadSafetyMode mode)
        {
            _innerLazy = new Lazy<Task<T>>(DefaultValue, mode);
        }

        public AsyncLazy(Func<T> valueFactory)
        {
            _innerLazy = new Lazy<Task<T>>(FromFuncT(valueFactory));
        }

        public AsyncLazy(Func<T> valueFactory, bool isThreadSafe)
        {
            _innerLazy = new Lazy<Task<T>>(FromFuncT(valueFactory), isThreadSafe);
        }

        public AsyncLazy(Func<T> valueFactory, LazyThreadSafetyMode mode)
        {
            _innerLazy = new Lazy<Task<T>>(FromFuncT(valueFactory), mode);
        }

        public AsyncLazy(Func<Task<T>> valueFactory)
        {
            _innerLazy = new Lazy<Task<T>>(valueFactory);
        }

        public AsyncLazy(Func<Task<T>> valueFactory, bool isThreadSafe)
        {
            _innerLazy = new Lazy<Task<T>>(valueFactory, isThreadSafe);
        }

        public AsyncLazy(Func<Task<T>> valueFactory, LazyThreadSafetyMode mode)
        {
            _innerLazy = new Lazy<Task<T>>(valueFactory, mode);
        }

        public override string ToString() => IsValueCreated ? GetValue().ToString() : _innerLazy.ToString();
        public bool IsValueCreated => _innerLazy.IsValueCreated && _innerLazy.Value.IsCompletedSuccessfully;
        public async Task<T> GetValue() => IsValueCreated ? await _innerLazy.Value : _innerLazy.Value.Result;
    }
}
