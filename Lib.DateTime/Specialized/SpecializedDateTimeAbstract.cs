﻿using System;
using System.Runtime.Serialization;
using Lib.Reflection;

namespace Lib.DateTime.Specialized
{
    /// <summary>
    /// Abstract DateTimeComponent, comparable to others, with operators from and to <see cref="_dateTime"/>
    /// </summary>
    public abstract class SpecializedDateTimeAbstract :
        IComparable,
        IComparable<System.DateTime>,
        IComparable<System.DateTime?>,
        IConvertible, 
        IEquatable<System.DateTime>, 
        IEquatable<System.DateTime?>,
        IFormattable, 
        ISerializable, 
        IEquatable<SpecializedDateTimeAbstract>,
        IComparable<SpecializedDateTimeAbstract>
    {
        private readonly System.DateTime? _dateTime;

        /// <summary>
        /// Constructor that checks the validity af the component
        /// </summary>
        protected SpecializedDateTimeAbstract(System.DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        /// <summary>
        /// Returns a <see cref="SpecializedDateTimeAbstract"/> child built from a DateTime, by implicit conversion
        /// </summary>
        public static TChild FromDateTime<TChild>(System.DateTime dateTime) where TChild : SpecializedDateTimeAbstract
        {
            var implicitOperator = typeof(TChild).GetImplicitOperator<System.DateTime>();
            return (TChild) implicitOperator.Invoke(null, new object[] {dateTime});
        }

        /// <inheritdoc />
        public bool Equals(SpecializedDateTimeAbstract other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _dateTime.Equals(other._dateTime);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is SpecializedDateTimeAbstract other && Equals(other);
        }

        /// <summary>
        /// Opérateur vers DateTime nullable
        /// </summary>
        public static implicit operator System.DateTime?(SpecializedDateTimeAbstract instance)
            => instance?._dateTime;

        /// <summary>
        /// Opérateur vers DateTime nullable
        /// </summary>
        public static implicit operator System.DateTime (SpecializedDateTimeAbstract instance)
            => instance?._dateTime ?? throw new NullReferenceException("Trying to create a DateTime from a null " + nameof(SpecializedDateTimeAbstract));

        /// <summary>
        /// Opérateur GreaterThan
        /// </summary>
        public static bool operator >(SpecializedDateTimeAbstract x, SpecializedDateTimeAbstract y)
            => x._dateTime > y._dateTime;

        /// <summary>
        /// Opérateur LessThan
        /// </summary>
        public static bool operator <(SpecializedDateTimeAbstract x, SpecializedDateTimeAbstract y)
            => x._dateTime < y._dateTime;

        /// <summary>
        /// Opérateur GreaterOrEqualThan
        /// </summary>
        public static bool operator >=(SpecializedDateTimeAbstract x, SpecializedDateTimeAbstract y)
            => x._dateTime >= y._dateTime;

        /// <summary>
        /// Opérateur LessOrEqualThan
        /// </summary>
        public static bool operator <=(SpecializedDateTimeAbstract x, SpecializedDateTimeAbstract y)
            => x._dateTime <= y._dateTime;

        /// <summary>
        /// Equality
        /// </summary>
        public static bool operator ==(SpecializedDateTimeAbstract left, SpecializedDateTimeAbstract right) => Equals(left, right);

        /// <summary>
        /// Inequality
        /// </summary>
        public static bool operator !=(SpecializedDateTimeAbstract left, SpecializedDateTimeAbstract right) => !Equals(left, right);

        /// <inheritdoc />
        public override int GetHashCode() => _dateTime.GetHashCode();

        /// <inheritdoc />
        // ReSharper disable once SpecifyACultureInStringConversionExplicitly
        public override string ToString() => _dateTime.ToString();

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
            => _dateTime.HasValue ? _dateTime.Value.ToString(format, formatProvider) : _dateTime.ToString();

        bool IEquatable<System.DateTime>.Equals(System.DateTime other) => _dateTime.HasValue && _dateTime.Value.Equals(other);
        bool IEquatable<System.DateTime?>.Equals(System.DateTime? other) => _dateTime.Equals(other);

        int IComparable.CompareTo(object obj)
        {
            if (!_dateTime.HasValue) return obj is null ? 0 : -1;
            return _dateTime.Value.CompareTo(obj);
        }

        int IComparable<System.DateTime>.CompareTo(System.DateTime other) => _dateTime?.CompareTo(other) ?? -1;

        int IComparable<System.DateTime?>.CompareTo(System.DateTime? other)
        {
            if (!_dateTime.HasValue) return other.HasValue ? -1 : 0;
            return -other?.CompareTo(_dateTime.Value) ?? 0;
        }

        int IComparable<SpecializedDateTimeAbstract>.CompareTo(SpecializedDateTimeAbstract other)
        {
            if (!_dateTime.HasValue) return other._dateTime.HasValue ? -1 : 0;
            return -other._dateTime?.CompareTo(_dateTime.Value) ?? 0;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) =>
            ((ISerializable)_dateTime)?.GetObjectData(info, context);

        TypeCode IConvertible.GetTypeCode() => TypeCode.DateTime;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convertible.ToBoolean(provider);
        byte IConvertible.ToByte(IFormatProvider provider) => Convertible.ToByte(provider);
        char IConvertible.ToChar(IFormatProvider provider) => Convertible.ToChar(provider);
        System.DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convertible.ToDateTime(provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convertible.ToDecimal(provider);
        double IConvertible.ToDouble(IFormatProvider provider) => Convertible.ToDouble(provider);
        short IConvertible.ToInt16(IFormatProvider provider) => Convertible.ToInt16(provider);
        int IConvertible.ToInt32(IFormatProvider provider) => Convertible.ToInt32(provider);
        long IConvertible.ToInt64(IFormatProvider provider) => Convertible.ToInt64(provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convertible.ToSByte(provider);
        float IConvertible.ToSingle(IFormatProvider provider) => Convertible.ToSingle(provider);
        string IConvertible.ToString(IFormatProvider provider) => Convertible.ToString(provider);
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convertible.ToType(conversionType, provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convertible.ToUInt16(provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convertible.ToUInt32(provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convertible.ToUInt64(provider);
        private IConvertible Convertible => _dateTime;
    }
}
