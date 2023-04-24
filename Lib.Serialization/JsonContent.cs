using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ConsoleApi.Serialization.Abstractions;

namespace ConsoleApi.Serialization;

public class JsonContent<T> : StringContent where T : JsonSerializationContract
{
    private readonly string _serialized;

    internal JsonContent(IJsonSerializer serializer, T toSerialize) 
        : this(serializer.Serialize(toSerialize))
    {
    }

    internal JsonContent(IJsonSerializer serializer, IEnumerable<T> toSerialize) 
        : this(serializer.Serialize(toSerialize))
    {
    }

    internal JsonContent(IJsonSerializer serializer, IEnumerable<T> toSerialize, string mediaType)
        : this(serializer.Serialize(toSerialize), mediaType)
    {
    }

    private JsonContent(string serialized, string mediaType = "application/json") : base(
        serialized,
        Encoding.UTF8,
        mediaType)
    {
        _serialized = serialized;
    }

    /// <inheritdoc />
    public override string ToString() => _serialized;
}

public static class JsonContentSerializerExtensions
{
    public static JsonContent<T> SerializeToContent<T>(this IJsonSerializer serializer, T content)
        where T : JsonSerializationContract => new(serializer, content);
    
    public static JsonContent<T> SerializeToContent<T>(this IJsonSerializer serializer, IEnumerable<T> content)
        where T : JsonSerializationContract => new (serializer, content);
    
    public static JsonContent<T> SerializeToContent<T>(this IJsonSerializer serializer, IEnumerable<T> content, string mediaType)
        where T : JsonSerializationContract => new (serializer, content, mediaType);
}