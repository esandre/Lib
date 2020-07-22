using System;
using Lib.DateTime.Descriptors.TimeSlot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lib.DateTime.Descriptors.Test
{
    [TestClass]
    public class TestYearlyRecurrenceRule
    {
        private static readonly IAliasProvider NoAliasProvider = Mock.Of<IAliasProvider>(MockBehavior.Strict);
        
        [TestMethod]
        public void TestBuilder()
        {
            {
                var proof = new YearlyRecurrentTimeSlot(11, 12, 13, 12);
                const string str = "EVERY YEAR BETWEEN 11/12 00:00 AND 13/12 23:59";
                var builded = new TimeSlotBuilder(NoAliasProvider).Build(str);

                Assert.AreEqual(proof, builded);
            }

            {
                var proof = new YearlyRecurrentTimeSlot(11, 12, new TimeSpan(8, 30, 0), 13, 12, new TimeSpan(9, 15, 0));
                const string str = "EVERY YEAR BETWEEN 11/12 08:30 AND 13/12 09:15";
                var builded = new TimeSlotBuilder(NoAliasProvider).Build(str);

                Assert.AreEqual(proof, builded);
            }
        }

        [TestMethod]
        public void TestContains()
        {
            {
                var rule = new YearlyRecurrentTimeSlot(1, 1, 31, 12);
                Assert.IsTrue(rule.Contains(System.DateTime.Now));
            }

            {
                var rule = new YearlyRecurrentTimeSlot(1, 1, new TimeSpan(12, 36, 59), 31, 12, new TimeSpan(23, 59, 9));
                Assert.IsFalse(rule.Contains(new System.DateTime(1, 1, 1, 12, 35, 45)));
            }
        }

        [TestMethod]
        public void TestToString()
        {
            {
                var proof = new YearlyRecurrentTimeSlot(11, 12, new TimeSpan(0), 13, 12, new TimeSpan(0));
                const string str = "EVERY YEAR BETWEEN 11/12 00:00 AND 13/12 00:00";

                Assert.AreEqual(str, proof.ToString());
            }

            {
                var proof = new YearlyRecurrentTimeSlot(11, 12, new TimeSpan(8, 30, 0), 13, 12, new TimeSpan(9, 15, 0));
                const string str = "EVERY YEAR BETWEEN 11/12 08:30 AND 13/12 09:15";

                Assert.AreEqual(str, proof.ToString());
            }
        }
    }
}
