using System.Globalization;
using Lib.DateTime.Descriptors.TimeSlot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lib.DateTime.Descriptors.Test
{
    [TestClass]
    public class TestAbsoluteTimeSlot
    {
        private static readonly IAliasProvider NoAliasProvider = Mock.Of<IAliasProvider>(MockBehavior.Strict);

        [TestMethod]
        public void TestContains()
        {
            {
                var rule = new AbsoluteTimeSlot(System.DateTime.Now, System.DateTime.MaxValue);
                Assert.IsTrue(rule.Contains(System.DateTime.Now));
            }

            {
                var rule = new AbsoluteTimeSlot(System.DateTime.MinValue, System.DateTime.Now.AddDays(-6));
                Assert.IsFalse(rule.Contains(System.DateTime.Now));
            }
        }

        [TestMethod]
        public void TestDescriptor()
        {
            {
                var proof = new AbsoluteTimeSlot(new System.DateTime(2015, 06, 12, 12, 21, 00), new System.DateTime(2015, 07, 03, 17, 04, 00));
                const string descriptor = "BETWEEN 12/06/2015 12:21 AND 03/07/2015 17:04";
                Assert.AreEqual(proof, new TimeSlotBuilder(NoAliasProvider).Build(descriptor));
            }

            {
                var proof = new AbsoluteTimeSlot(new System.DateTime(2015, 06, 12, 12, 21, 55), new System.DateTime(2015, 06, 18));
                const string descriptor = "BETWEEN 12/06/2015 12:21 AND 18/06/2015 00:00";
                Assert.AreEqual(proof, new TimeSlotBuilder(NoAliasProvider).Build(descriptor));
            }
        }

        [TestMethod]
        public void TestFrench()
        {
            {
                var proof = new AbsoluteTimeSlot(new System.DateTime(2015, 06, 12, 12, 21, 00), new System.DateTime(2015, 07, 03, 17, 04, 00));
                const string descriptor = "ENTRE 12/06/2015 12:21 ET 03/07/2015 17:04";
                Assert.AreEqual(proof, new TimeSlotBuilder(NoAliasProvider, new CultureInfo("fr")).Build(descriptor));
            }

            {
                var proof = new AbsoluteTimeSlot(new System.DateTime(2015, 06, 12, 12, 21, 55), new System.DateTime(2015, 06, 18));
                const string descriptor = "ENTRE 12/06/2015 12:21 ET 18/06/2015 00:00";
                Assert.AreEqual(proof, new TimeSlotBuilder(NoAliasProvider, new CultureInfo("fr")).Build(descriptor));
            }
        }
    }
}
