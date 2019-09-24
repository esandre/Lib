using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.DateTime.Descriptors.Test
{
    [TestClass]
    public class TestDate
    {
        [TestMethod]
        public void TestCreation()
        {
            var date = new Date(2015, 05, 16);
            Assert.AreEqual(2015, date.Year);
            Assert.AreEqual(05, date.Month);
            Assert.AreEqual(16, date.Day);
        }

        [TestMethod]
        public void TestConversion()
        {
            var datetime = new System.DateTime(2015, 02, 03, 15, 18, 47);
            var proof = new Date(2015, 02, 03);

            Assert.AreEqual(proof, datetime.ToDate());
        }
    }
}
