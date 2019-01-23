using System.IO;
using System.Text;
using Lib.Hash.Hashable.ByteExtractors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Lib.Hash.Hashable.Test
{
    [TestClass]
    public class StringByteExtractorTest
    {
        [TestMethod]
        public void String_Encoded()
        {
            Check.That(ExtractBytes("Test1")).Equals(Encoding.Default.GetBytes("Test1"));
            Check.That(ExtractBytes("Test2")).Equals(Encoding.Default.GetBytes("Test2"));
            Check.That(ExtractBytes("Test1")).Not.Equals(Encoding.Default.GetBytes("Test2"));
        }

        private static byte[] ExtractBytes(object instance)
        {
            using (var stream = new MemoryStream())
            {
                var extractor = new StringByteExtractor();
                extractor.Extract(instance, stream);
                return stream.ToArray();
            }
        }
    }
}
