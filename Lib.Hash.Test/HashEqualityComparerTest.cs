using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lib.Hash.Test
{
    [TestClass]
    public class HashEqualityComparerTest
    {
        [TestMethod]
        public void TwoHashes_WithSameData_AreSame()
        {
            var inputX = new byte[] { 0, 1, 2, 3, 4 };
            var inputY = new byte[] { 0, 1, 2, 3, 4 };

            var equalityComparer = new HashEqualityComparer();

            var x = Mock.Of<IHash>(m => m.Bytes == inputX);
            var y = Mock.Of<IHash>(m => m.Bytes == inputY);
            
            Assert.IsTrue(equalityComparer.Equals(x, y));
        }

        [TestMethod]
        public void TwoHashes_WithDifferentData_AreDifferent()
        {
            var inputX = new byte[] { 0, 1, 2, 3, 4 };
            var inputY = new byte[] { 1, 2, 3, 4, 5 };

            var equalityComparer = new HashEqualityComparer();

            var x = Mock.Of<IHash>(m => m.Bytes == inputX);
            var y = Mock.Of<IHash>(m => m.Bytes == inputY);

            Assert.IsFalse(equalityComparer.Equals(x, y));
        }
    }
}
