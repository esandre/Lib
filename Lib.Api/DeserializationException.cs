using System;
using System.IO;
using JetBrains.Annotations;

#pragma warning disable 1591

namespace Lib.Api
{
    [Obsolete]
    public class DeserializationException : Exception
    {
        [PublicAPI]
        public string RawResponse { get; }

        public DeserializationException(Type expectedContract, Stream rawResponse, Exception inner) 
            : base("Deserialization failed for contract " + expectedContract.Name, inner)
        {
            using (var reader = new StreamReader(rawResponse))
            {
                this.RawResponse = reader.ReadToEnd();
            }
        }

        public override string ToString()
        {
            return base.ToString() 
                   + Environment.NewLine 
                   + "Raw response dump :" 
                   + Environment.NewLine 
                   + this.RawResponse;
        }
    }
}
