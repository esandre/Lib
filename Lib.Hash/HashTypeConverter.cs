using System;
using System.ComponentModel;
using System.Globalization;

namespace Lib.Hash
{
    /// <summary>
    /// TypeConverter for <see cref="IHash"/>
    /// </summary>
    public class HashTypeConverter : TypeConverter
    {
        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertFrom(context, destinationType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return value is string inputString 
                ? HashStringRepresentation.Retrieve(inputString) 
                : base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is IHash hash)
                return HashStringRepresentation.Process(hash);
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}