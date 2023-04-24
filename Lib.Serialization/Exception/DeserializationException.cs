using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ConsoleApi.Serialization.Exception;

[PublicAPI]
public class DeserializationException : System.Exception
{
    public string Json { get; }
    public Stream JsonStream { get; }
    public Type ContractType { get; }

    private static string TruncateIfTooLong(string input) => input.Length > 512 ? input[..512] : input;

    public DeserializationException(string json, Type contractType, System.Exception e)
        : base($"Cannot deserialize JSON {TruncateIfTooLong(json)} into contract {contractType}", e)
    {
        Json = json;
        ContractType = contractType;
        JsonStream = new MemoryStream(Encoding.Default.GetBytes(json));
    }

    public static async Task<DeserializationException> FromStreamAsync(
        Stream jsonStream, 
        Type contractType, 
        System.Exception e)
    {
        string buffer;

        try
        {
            if (jsonStream.Position != 0 && jsonStream.CanSeek)
                jsonStream.Position = 0;

            using var reader = new StreamReader(jsonStream);
            buffer = await reader.ReadToEndAsync();
        } 
        catch
        {
            buffer = "<Unreadable>";
        }

        return new DeserializationException(buffer, contractType, e);
    }
}