using System;
using System.Threading.Tasks;

namespace Lib.Patterns
{
    public class Existe<T> : IEquatable<T>, IEquatable<Existe<T>>
    {
        private readonly T _element;
        private readonly ICheckExistence<T> _checker;

        public bool IsPresent => _checker.Exists(_element);

        public T SelectIfPresentOrThrow()
        {
            if(!IsPresent)
                throw _checker.BuildExceptionIfNotExists(_element);
            return _element;
        }

        public async Task SelectAsync(Func<T, Task> present, Func<Task> absent)
        {
            if (IsPresent) await present(_element);
            else await absent();
        }

        public Existe(T element, ICheckExistence<T> checker)
        {
            _element = element;
            _checker = checker;
        }

        public bool Equals(Existe<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._element);
        }

        public bool Equals(T other) => _element.Equals(other);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj switch
            {
                Existe<T> existe => Equals(existe),
                T element => Equals(element),
                _ => false
            };
        }

        public override int GetHashCode() => _element.GetHashCode();
        public override string ToString() => _element.ToString();
    }

    public class ExisteAsync<T> : IEquatable<T>, IEquatable<ExisteAsync<T>>
    {
        private readonly T _element;
        private readonly ICheckExistenceAsync<T> _checker;

        public async Task<bool> IsPresentAsync() => await _checker.ExistsAsync(_element);

        public async Task<T> SelectIfPresentOrThrowAsync()
        {
            if(!await IsPresentAsync())
                throw _checker.BuildExceptionIfNotExists(_element);
            return _element;
        }

        public async Task SelectAsync(Func<T, Task> present, Func<Task> absent)
        {
            if (await IsPresentAsync()) await present(_element);
            else await absent();
        }

        public ExisteAsync(T element, ICheckExistenceAsync<T> checker)
        {
            _element = element;
            _checker = checker;
        }

        public bool Equals(ExisteAsync<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._element);
        }

        public bool Equals(T other) => _element.Equals(other);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj switch
            {
                Existe<T> existe => Equals(existe),
                T element => Equals(element),
                _ => false
            };
        }

        public override int GetHashCode() => _element.GetHashCode();
        public override string ToString() => _element.ToString();
    }
}
