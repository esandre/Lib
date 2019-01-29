using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lib.Hash.Hashable.ByteExtractors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Lib.Hash.Hashable.Test
{
    [TestClass]
    public class EnumerableByteExtractorTest
    {
        [TestMethod]
        public void Empty_Array_WritesNothing()
        {
            var elementExtractorMock = new Mock<IByteExtractor>();
            elementExtractorMock.Setup(m => m.Extract(It.IsAny<object>(), It.IsAny<Stream>()))
                .Callback<object, Stream>((obj, stream) => stream.Write(new byte[] { 1 }));
            var elementExtractor = elementExtractorMock.Object;

            var arrayExtractor = new EnumerableByteExtractor(elementExtractor);

            using (var stream = new MemoryStream())
            {
                arrayExtractor.Extract(new object[0], stream);
                Check.That(stream.ToArray()).IsEmpty();
            }
        }

        [TestMethod]
        public void Empty_Enumerable_WritesNothing()
        {
            var elementExtractorMock = new Mock<IByteExtractor>();
            elementExtractorMock.Setup(m => m.Extract(It.IsAny<object>(), It.IsAny<Stream>()))
                .Callback<object, Stream>((obj, stream) => stream.Write(new byte[] { 1 }));
            var elementExtractor = elementExtractorMock.Object;

            var arrayExtractor = new EnumerableByteExtractor(elementExtractor);

            using (var stream = new MemoryStream())
            {
                arrayExtractor.Extract(new List<object>(), stream);
                Check.That(stream.ToArray()).IsEmpty();
            }
        }

        [TestMethod]
        public void Concatenates_AllArrayElements_Bytes()
        {
            var element = new object();
            var elementBytes = new[] {(byte) 0x01};

            var elementExtractorMock = new Mock<IByteExtractor>();
            elementExtractorMock.Setup(m => m.Extract(It.IsAny<object>(), It.IsAny<Stream>()))
                .Callback<object, Stream>((obj, stream) => stream.Write(elementBytes));
            var elementExtractor = elementExtractorMock.Object;

            var arrayExtractor = new EnumerableByteExtractor(elementExtractor);

            using (var stream = new MemoryStream())
            {
                const int repeats = 1;

                arrayExtractor.Extract(Enumerable.Repeat(element, repeats).ToArray(), stream);
                var expected = Enumerable.Repeat(elementBytes, repeats).SelectMany(x => x);
                Check.That(stream.ToArray()).ContainsExactly(expected);
            }

            using (var stream = new MemoryStream())
            {
                const int repeats = 10;

                arrayExtractor.Extract(Enumerable.Repeat(element, repeats).ToArray(), stream);
                var expected = Enumerable.Repeat(elementBytes, repeats).SelectMany(x => x);
                Check.That(stream.ToArray()).ContainsExactly(expected);
            }
        }

        [TestMethod]
        public void Concatenates_AllEnumerableElements_Bytes()
        {
            var element = new object();
            var elementBytes = new[] { (byte)0x01 };

            var elementExtractorMock = new Mock<IByteExtractor>();
            elementExtractorMock.Setup(m => m.Extract(It.IsAny<object>(), It.IsAny<Stream>()))
                .Callback<object, Stream>((obj, stream) => stream.Write(elementBytes));
            var elementExtractor = elementExtractorMock.Object;

            var arrayExtractor = new EnumerableByteExtractor(elementExtractor);

            using (var stream = new MemoryStream())
            {
                const int repeats = 1;

                arrayExtractor.Extract(Enumerable.Repeat(element, repeats), stream);
                var expected = Enumerable.Repeat(elementBytes, repeats).SelectMany(x => x);
                Check.That(stream.ToArray()).ContainsExactly(expected);
            }

            using (var stream = new MemoryStream())
            {
                const int repeats = 10;

                arrayExtractor.Extract(Enumerable.Repeat(element, repeats), stream);
                var expected = Enumerable.Repeat(elementBytes, repeats).SelectMany(x => x);
                Check.That(stream.ToArray()).ContainsExactly(expected);
            }
        }
    }
}
