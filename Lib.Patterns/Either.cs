using System;

namespace Lib.Patterns
{
    public class Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>
    {
        private Maybe<TLeft> Left { get; }
        private Maybe<TRight> Right { get; }
        
        public void Select(Action<TLeft> ifLeft, Action<TRight> ifRight)
        {
            Left.SelectIfHasItem(ifLeft);
            Right.SelectIfHasItem(ifRight);
        }

        public Either<TDestinationLeft, TDestinationRight> Transform<TDestinationLeft, TDestinationRight>(
            Func<TLeft, TDestinationLeft> leftTransform,
            Func<TRight, TDestinationRight> rightTransform)
        {
            return Select(
                left => new Either<TDestinationLeft, TDestinationRight>(leftTransform(left)), 
                right => new Either<TDestinationLeft, TDestinationRight>(rightTransform(right))
            );
        }

        public T Select<T>(Func<TLeft, T> ifLeft, Func<TRight, T> ifRight)
        {
            T result = default;

            if (Left.HasItem)
                Left.SelectIfHasItem(left => result = ifLeft(left));

            if (Right.HasItem)
                Right.SelectIfHasItem(right => result = ifRight(right));

            return result;
        }

        public Either(TLeft value)
        {
            Left = new Maybe<TLeft>(value);
            Right = new Maybe<TRight>();
        }

        public Either(TRight value)
        {
            Left = new Maybe<TLeft>();
            Right = new Maybe<TRight>(value);
        }

        public override string ToString() => Select(left => left.ToString(), right => right.ToString());
        public override int GetHashCode() => Select(left => left.GetHashCode(), right => right.GetHashCode());

        public bool Equals(Either<TLeft, TRight> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Select(
                left => other.Select(otherLeft => left.Equals(otherLeft), right => false), 
                right => other.Select(left => false, otherRight => right.Equals(otherRight))
            );
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((Either<TLeft, TRight>) obj);
        }

        public static bool operator ==(Either<TLeft, TRight> left, Either<TLeft, TRight> right) => left?.Equals(right) ?? right is null;

        public static bool operator !=(Either<TLeft, TRight> left, Either<TLeft, TRight> right) => !(left == right);
    }
}
