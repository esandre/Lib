using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Lib.Random;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Lib.Hash.Test
{
    [TestClass]
    public abstract class StreamHashAbstractTest<THashAlgorithm> 
        where THashAlgorithm : HashAlgorithm
    {
        private readonly System.Random _random = new System.Random();

        protected abstract THashAlgorithm Instance { get; }

        [TestMethod]
        public void Hashes_WithSameAlgorithm_CanHashParallel()
        {
            var testResults = Enumerable.Repeat(0, 1000)
                .Select(_ =>
                {
                    var randomBytes = _random.NextBytes(100);

                    var inputStreamForExpected = new MemoryStream(randomBytes);
                    var inputStreamForHash = new MemoryStream(randomBytes);

                    var expectedByteArray = Instance.ComputeHash(inputStreamForExpected);

                    return (expected: expectedByteArray, dataToTest: inputStreamForHash);
                })
                .ToArray()
                .Select(tuple =>
                {
                    var (expectedByteArray, inputStreamForHash) = tuple;
                    var hashMock = new Mock<StreamHash>(
                            inputStreamForHash,
                            Instance)
                        { CallBase = true }.Object;
                    return (expected: expectedByteArray, actual: hashMock.Bytes);
                })
                .AsParallel().ToArray();

            foreach (var (expected, actual) in testResults)
            {
                Check.That(actual).ContainsExactly(expected);
            }
        }

        [TestMethod]
        public void AHash_Bytes_AreHashedOutput_OfInputDataStream()
        {
            var testingBytes = new byte[] {0, 1, 2, 3, 4};

            var inputStreamForExpected = new MemoryStream(testingBytes);
            var inputStreamForHash = new MemoryStream(testingBytes);

            var expectedByteArray = Instance.ComputeHash(inputStreamForExpected);

            var hashMock = new Mock<StreamHash>(
                    inputStreamForHash,
                    Instance)
                { CallBase = true }.Object;

            Check.That(hashMock.Bytes).ContainsExactly(expectedByteArray);
        }

        [TestMethod]
        public void AHash_EqualityOperator_RelaysEqualsResult()
        {
            var hashMock = new Mock<IHash>();
            hashMock.SetupSequence(m => m.Equals(It.IsAny<IHash>()))
                .Returns(true)
                .Returns(false);
            var hash = hashMock.Object;

            Check.That(hash.Equals(It.IsAny<IHash>())).Equals(true);
            Check.That(hash.Equals(It.IsAny<IHash>())).Equals(false);

            hashMock.Verify(m => m.Equals(It.IsAny<IHash>()), Times.Exactly(2));
        }

        [TestMethod]
        public void AHash_StringRepresentation_ReturnsHashHexadecimalRepresentation_Value()
        {
            var inputStream = new MemoryStream(new byte[] { 0, 1, 2, 3, 4 });

            var hash = new Mock<StreamHash>(
                inputStream, 
                Mock.Of<THashAlgorithm>()) { CallBase = true }.Object;

            var expectedRepresentation = HashStringRepresentation.Process(hash);

            Check.That(hash.ToString()).Equals(expectedRepresentation);
        }
    }
}
