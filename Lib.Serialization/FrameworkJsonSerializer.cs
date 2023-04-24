using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApi.Serialization.Abstractions;
using ConsoleApi.Serialization.Exception;
using JetBrains.Annotations;

namespace ConsoleApi.Serialization;

public class FrameworkJsonSerializer : IJsonSerializer
{
    [CanBeNull] private readonly JsonSerializerOptions _options;

    public FrameworkJsonSerializer() : this(null)
    {
    }

    public FrameworkJsonSerializer(JsonSerializerOptions options)
    {
        _options = options;
    }

    public string Serialize<T>(T contract) where T : JsonSerializationContract
    {
        try
        {
            return JsonSerializer.Serialize(contract, _options);
        }
        catch (System.Exception e)
        {
            throw new SerializationException(contract, e);
        }
    }

    public string Serialize(DateTime contract)
    {
        try
        {
            return JsonSerializer.Serialize(contract, _options);
        }
        catch (System.Exception e)
        {
            throw new SerializationException(contract, e);
        }
    }

    public string Serialize<T>(IEnumerable<T> contract) where T : JsonSerializationContract
    {
        try
        {
            return JsonSerializer.Serialize(contract, _options);
        }
        catch (System.Exception e)
        {
            throw new SerializationException(contract, e);
        }
    }

    public T Deserialize<T>(string representation) where T : JsonDeserializationContract
    {
        try
        {
            return JsonSerializer.Deserialize<T>(representation, _options);
        } 
        catch(System.Exception e)
        {
            throw new DeserializationException(representation, typeof(T), e);
        }
    }

    public DateTime Deserialize(string representation)
    {
        try
        {
            return JsonSerializer.Deserialize<DateTime>(representation, _options);
        }
        catch (System.Exception e)
        {
            throw new DeserializationException(representation, typeof(DateTime), e);
        }
    }

    public async Task<T> DeserializeAsync<T>(Stream stream, CancellationToken token) where T : JsonDeserializationContract
    {
        try
        {
            if (stream.CanSeek && stream.Position != 0) stream.Position = 0;
            return await JsonSerializer.DeserializeAsync<T>(stream, _options, token);
        }
        catch (System.Exception e)
        {
            throw await DeserializationException.FromStreamAsync(stream, typeof(T), e);
        }
    }

    public async Task<IEnumerable<T>> DeserializeManyAsync<T>(Stream stream, CancellationToken token) where T : JsonDeserializationContract
    {
        try
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<T>>(stream, _options, token);
        }
        catch (System.Exception e)
        {
            throw await DeserializationException.FromStreamAsync(stream, typeof(T), e);
        }
    }

    /// <inheritdoc />
    public IEnumerable<T> DeserializeMany<T>(string json) where T : JsonDeserializationContract
    {
        try
        {
            return JsonSerializer.Deserialize<IEnumerable<T>>(json, _options);
        }
        catch (System.Exception e)
        {
            throw new DeserializationException(json, typeof(IEnumerable<T>), e);
        }
    }

    public string SerializeUnchecked(object objectToSerialize)
    {
        try
        {
            return JsonSerializer.Serialize(objectToSerialize, _options);
        }
        catch (System.Exception e)
        {
            throw new SerializationException(objectToSerialize, e);
        }
    }
}