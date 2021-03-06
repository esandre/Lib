﻿using System;
using System.Collections.Generic;

namespace Lib.Encapsulation
{
    /// <summary>
    /// Decorated a string
    /// </summary>
    public abstract class EncapsulatedValueAbstract<TValue> : IEncapsulatedValue<TValue>, IEquatable<EncapsulatedValueAbstract<TValue>>
    {
        /// <inheritdoc />
        public TValue Value { get; }

        /// <summary>
        /// Constructor from other instance
        /// </summary>
        protected EncapsulatedValueAbstract(IEncapsulatedValue<TValue> other)
        {
            Value = other.Value;
        }

        /// <summary>
        /// Constructor from string
        /// </summary>
        protected EncapsulatedValueAbstract(TValue input)
        {
            Value = input;
        }

        /// <summary>
        /// Comparer used for values
        /// </summary>
        protected virtual IEqualityComparer<TValue> ValueComparer => EqualityComparer<TValue>.Default;

        /// <inheritdoc />
        public override string ToString() => Value.ToString();

        /// <inheritdoc />
        public bool Equals(EncapsulatedValueAbstract<TValue> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;
            return ValueComparer.Equals(Value, other.Value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return ValueComparer.Equals(Value, ((EncapsulatedValueAbstract<TValue>) obj).Value);
        }

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(GetType(), ValueComparer.GetHashCode(Value));

        /// <summary>
        /// Equality operator
        /// </summary>
        public static bool operator ==(EncapsulatedValueAbstract<TValue> left, EncapsulatedValueAbstract<TValue> right) => Equals(left, right);

        /// <summary>
        /// Equality operator
        /// </summary>
        public static bool operator !=(EncapsulatedValueAbstract<TValue> left, EncapsulatedValueAbstract<TValue> right) => !Equals(left, right);
    }
}
