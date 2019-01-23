using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Lib.Hash.Test
{
    [TestClass]
    public class HashStringRepresentationTest
    {
        [TestMethod]
        public void TheHexRepresentation_IsHexadecimalRepresentation_OfBytes()
        {
            var bytes = new byte[] { 0, 1, 2, 3, 4 };
            var expectedRepresentation = BitConverter.ToString(bytes).Replace("-", "");
            
            var hashMock = Mock.Of<IHash>(m => m.Bytes == bytes);
            var hexRepresentation = HashStringRepresentation.Process(hashMock);

            Check.That(hexRepresentation).Equals(expectedRepresentation);
        }
    }
}
