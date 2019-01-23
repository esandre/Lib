using System;
using System.Diagnostics;
using System.Text;

namespace Lib.Hash.Hashable.ByteExtractors
{
    internal class StringByteExtractor : IByteExtractor
    {
        public bool CanExtract(Type t) => t == typeof(string);

        public void Extract(object instance, System.IO.Stream stream)
        {
            var str = instance as string;
            Debug.Assert(str != null, nameof(str) + " != null");

            var bytes = Encoding.Default.GetBytes(str);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
