using System;
using System.Threading.Tasks;

namespace Lib.Patterns
{
    public class Maybe
    {
        private object Item { get; }
        public bool HasItem { get; }

        protected Maybe()
        {
            HasItem = false;
        }

        protected Maybe(object item)
        {
            HasItem = true;
            Item = item;
        }

        public void IfAbsent(Action action)
        {
            if (!HasItem) action();
        }

        public TReturn Select<TReturn>(Func<object, TReturn> ifPresent, Func<TReturn> ifAbsent) 
            => HasItem ? ifPresent(Item) : ifAbsent();
    }

    public class Maybe<T> : Maybe, IEquatable<Maybe<T>>, IEquatable<T>
    {
        private T Item { get; }

        public Maybe(T item) : base(item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            Item = item;
        }

        public Maybe()
        {
        }

        public void SelectIfHasItem(Action<T> selector)
        {
            if(HasItem) selector(Item);
        }

        public T SelectIfHasItemOrThrow(Func<Exception> e)
        {
            if (HasItem) return Item;
            throw e();
        }

        public async Task SelectIfHasItemAsync(Func<T, Task> selector)
        {
            if (HasItem) await selector(Item);
        }

        public TReturn Select<TReturn>(Func<T, TReturn> ifPresent, Func<TReturn> ifAbsent)
        {
            return HasItem ? ifPresent(Item) : ifAbsent();
        }

        public void Select(Action<T> ifPresent, Action ifAbsent)
        {
            if (HasItem) ifPresent(Item);
            else ifAbsent();
        }

        public async Task<TReturn> SelectAsync<TReturn>(Func<T, Task<TReturn>> ifPresent, Func<Task<TReturn>> ifAbsent)
        {
            return HasItem ? await ifPresent(Item) : await ifAbsent();
        }

        public async Task SelectAsync(Func<T, Task> ifPresent, Action ifAbsent)
        {
            if (HasItem) await ifPresent(Item);
            else ifAbsent();
        }

        public Maybe<TReturn> Transform<TReturn>(Func<T, TReturn> transformation) 
            => HasItem ? new Maybe<TReturn>(transformation(Item)) : new Maybe<TReturn>();

        public Maybe<TReturn> Transform<TReturn>(Func<T, Maybe<TReturn>> transformation)
            => HasItem ? transformation(Item) : new Maybe<TReturn>();

        public bool Equals(Maybe<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (HasItem == false) return !other.HasItem;

            return Equals(other.Item);
        }

        public bool Equals(T other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(Item, other)) return true;
            if (HasItem == false) return false;

            return Item.Equals(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj switch
            {
                Maybe<T> maybe => Equals(maybe),
                T element => Equals(element),
                _ => false
            };
        }

        public override int GetHashCode() => HasItem ? Item.GetHashCode() : typeof(Maybe<T>).GetHashCode();
        public static bool operator ==(Maybe<T> left, Maybe<T> right) => Equals(left, right);
        public static bool operator !=(Maybe<T> left, Maybe<T> right) => !Equals(left, right);
        public override string ToString() => Select(item => item.ToString(), () => $"Empty Maybe<{typeof(T)}>");
    }
}
