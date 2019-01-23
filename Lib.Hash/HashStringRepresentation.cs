using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Hash
{

    /// <summary>
    /// StringRepresentation of a <see cref="IHash"/>
    /// </summary>
    public class HashStringRepresentation
    {
        /// <summary>
        /// Retrieves a Hash from a String representation
        /// </summary>
        public static IHash Retrieve(string representation)
            => new ArbitraryHash(RetrieveFromRepresentation(representation).ToArray());

        /// <summary>
        /// Processes a Hash into a String
        /// </summary>
        public static string Process(IHash hash) => GetRepresentation(hash.Bytes);

        private static string GetRepresentation(byte[] bytes) =>
            BitConverter.ToString(bytes).Replace("-", "");

        private static IEnumerable<string> SeparateInChunks(string str, int chunkSize)
        {
            if (str.Length % 2 != 0) throw new FormatException("Length is not a multiple of " + chunkSize);

            for (var index = 0; index < str.Length; index += chunkSize)
                yield return str.Substring(index, chunkSize);
        }

        private static IEnumerable<byte> RetrieveFromRepresentation(string representation)
        {
            representation = representation.Replace("-", "");
            return SeparateInChunks(representation, 2).Select(chunk => Convert.ToByte(chunk, 16));
        }
    }
}
