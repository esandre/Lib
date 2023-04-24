using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApi.Serialization.Exception;

namespace ConsoleApi.Serialization.Abstractions;

public interface IJsonSerializer
{
    /// <summary>
    /// Serializes a contract <typeparam name="T">T</typeparam> into its JSON representation
    /// </summary>
    /// <exception cref="SerializationException"></exception>
    string Serialize<T>(T contract) where T : JsonSerializationContract;

    /// <summary>
    /// Serializes a <see cref="DateTime"/> into its JSON representation
    /// </summary>
    /// <exception cref="SerializationException"></exception>
    string Serialize(DateTime contract);

    /// <summary>
    /// Serializes a collection of contracts <typeparam name="T">T</typeparam> into its JSON representation
    /// </summary>
    /// <exception cref="SerializationException"></exception>
    string Serialize<T>(IEnumerable<T> contract) where T : JsonSerializationContract;

    /// <summary>
    /// Please, don't use this for regular JSON sending jobs. It's mainly for logging purposes.
    /// </summary>
    /// <exception cref="SerializationException"></exception>
    string SerializeUnchecked(object objectToSerialize);

    /// <exception cref="DeserializationException"></exception>
    T Deserialize<T>(string representation) where T : JsonDeserializationContract;

    /// <exception cref="DeserializationException"></exception>
    DateTime Deserialize(string representation);
        
    /// <exception cref="DeserializationException"></exception>
    Task<T> DeserializeAsync<T>(Stream stream, CancellationToken token) where T : JsonDeserializationContract;

    /// <exception cref="DeserializationException"></exception>
    Task<IEnumerable<T>> DeserializeManyAsync<T>(Stream stream, CancellationToken token)
        where T : JsonDeserializationContract;

    /// <exception cref="DeserializationException"></exception>
    IEnumerable<T> DeserializeMany<T>(string json)
        where T : JsonDeserializationContract;
}