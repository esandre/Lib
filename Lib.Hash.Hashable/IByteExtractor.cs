using System;

namespace Lib.Hash.Hashable
{
    /// <summary>
    /// Extracts bytes from instances
    /// </summary>
    public interface IByteExtractor
    {
        /// <summary>
        /// True if the instance can extract
        /// </summary>
        bool CanExtract(Type t);

        /// <summary>
        /// Extracts bytes from instance
        /// </summary>
        void Extract(object instance, System.IO.Stream stream);
    }
}
