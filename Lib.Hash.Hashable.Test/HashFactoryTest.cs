using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Lib.Hash.Hashable.Test
{
    public class HashFactoryTest<THashAlgorithm> where THashAlgorithm : HashAlgorithm
    {
        private static readonly MethodInfo CreateMethod = typeof(THashAlgorithm)
            .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Single(method => method.Name == "Create" && !method.GetParameters().Any());

        private static THashAlgorithm Algorithm => (THashAlgorithm) CreateMethod.Invoke(null, new object[0]);

        [TestMethod]
        public void Factory_Anything_Hashes_Result_Of_ByteExtractor_IfCanExtract()
        {
            var bytes = new byte[] {1};

            var byteExtractorMock = new Mock<IByteExtractor>();
            byteExtractorMock.Setup(m => m.CanExtract(It.IsAny<Type>())).Returns(true);
            byteExtractorMock.Setup(m => m.Extract(It.IsAny<object>(), It.IsAny<System.IO.Stream>()))
                .Callback((object obj, System.IO.Stream stream) => stream.Write(bytes));
            var byteExtractor = byteExtractorMock.Object;

            var hashFactory = new HashFactory<THashAlgorithm>(() => Algorithm, byteExtractor);

            var expected = Algorithm.ComputeHash(bytes);
            var actual = hashFactory.Factory(new object()).Bytes;
            Check.That(actual).ContainsExactly(expected);
        }

        [TestMethod]
        public void Factory_Anything_Throws_ArgumentException_IfCannotExtract()
        {
            var byteExtractor = Mock.Of<IByteExtractor>(m => m.CanExtract(It.IsAny<Type>()) == false);
            var hashFactory = new HashFactory<THashAlgorithm>(() => Algorithm, byteExtractor);
            
            var obj = new object();

            Check.ThatCode(() => hashFactory.Factory(obj))
                .Throws<ArgumentException>()
                .WithMessage(HashFactory<THashAlgorithm>.CannotHashMessage(obj));
        }
    }

    [TestClass] public class SHA512HashFactoryTest : HashFactoryTest<SHA512> { }
}
