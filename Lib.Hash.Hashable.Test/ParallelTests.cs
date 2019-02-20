using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Lib.Hash.Hashable.Test
{
    [TestClass]
    public class ParallelTests
    {
        [TestMethod]
        public void NoCollisions_ParallelOrSingleThread()
        {
            var byteExtractorMock = new Mock<IByteExtractor>();
            byteExtractorMock.Setup(m => m.CanExtract(typeof(int))).Returns(true);
            byteExtractorMock.Setup(m => m.Extract(It.IsAny<object>(), It.IsAny<Stream>()))
                .Callback<object, Stream>((i, stream) =>
                {
                    stream.Write(BitConverter.GetBytes((int) i));
                });

            var datas = Enumerable.Range(0, 10).ToArray();
            var hashFactory = new HashFactory<SHA512>(SHA512.Create, byteExtractorMock.Object);

            var results = datas.Select(data => hashFactory.Factory(data)).ToArray();
            var parallelResults = datas.AsParallel().Select(data => hashFactory.Factory(data)).ToArray();

            Check.That(results).ContainsNoDuplicateItem();
            Check.That(parallelResults).ContainsNoDuplicateItem();
            Check.That(parallelResults).Contains(results);
        }
    }
}
