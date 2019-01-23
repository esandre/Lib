using System.Security.Cryptography;

namespace Lib.Hash.Stream
{
    /// <summary>
    /// A SHA512 Hash
    /// </summary>
    public class SHA512Hash : StreamHash<SHA512>
    {
        private static readonly SHA512 SHA512Instance = SHA512.Create();

        /// <summary>
        /// Constructor
        /// </summary>
        public SHA512Hash(System.IO.Stream inputStream) : base(inputStream, SHA512Instance)
        {
        }
    }
}
