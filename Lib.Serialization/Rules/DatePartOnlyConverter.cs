using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApi.Serialization.Rules;

/// <inheritdoc />
/// <summary>
/// Simple datetime converter to json convert to "simple"
/// </summary>
public class DatePartOnlyConverter : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-dd";

    /// <inheritdoc />
    /// <summary>
    /// Read value and transform it to DateTime?
    /// </summary>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var val = reader.GetString() ?? throw new ArgumentException(nameof(reader));
        return DateTime.Parse(val).Date;
    }

    /// <inheritdoc />
    /// <summary>
    /// Write value to correct JSON format
    /// </summary>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
    }
}