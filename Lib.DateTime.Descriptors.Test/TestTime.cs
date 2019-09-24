using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.DateTime.Descriptors.Test
{
    [TestClass]
    public class TestTime
    {
        [TestMethod]
        public void TestCreation()
        {
            var date = new Time(16, 59, 08);
            Assert.AreEqual(16, date.Hour);
            Assert.AreEqual(59, date.Minute);
            Assert.AreEqual(08, date.Second);
        }

        [TestMethod]
        public void TestConversion()
        {
            var datetime = new System.DateTime(2015, 02, 03, 15, 18, 47);
            var proof = new Time(15, 18, 47);

            Assert.AreEqual(proof, datetime.ToTime());
        }
    }
}
