using JetBrains.Annotations;

namespace ConsoleApi.Serialization.Exception;

[PublicAPI]
public class SerializationException : System.Exception
{
    public SerializationException(object contract, System.Exception e) 
        : base($"Cannot serialize contract {contract}", e)
    {
        Contract = contract;
    }

    public object Contract { get; }
}