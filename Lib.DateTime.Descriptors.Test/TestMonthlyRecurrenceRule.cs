using System;
using Lib.DateTime.Descriptors.TimeSlot;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.DateTime.Descriptors.Test
{
    [TestClass]
    public class TestMonthlyRecurrenceRule
    {
        [TestMethod]
        public void TestBuilder()
        {
            {
                var proof = new MonthlyRecurrentTimeSlot(16, 24);
                const string str = "EVERY MONTH BETWEEN 16 AND 24";
                var builded = new TimeSlotBuilder().Build(str);

                Assert.AreEqual(proof, builded);
            }

            {
                var proof = new MonthlyRecurrentTimeSlot(1, 12, new TimeSpan(8, 30, 00), new TimeSpan(9, 15, 00));
                const string str = "EVERY MONTH BETWEEN 1 08:30 AND 12 09:15";
                var builded = new TimeSlotBuilder().Build(str);

                Assert.AreEqual(proof, builded);
            }
        }

        [TestMethod]
        public void TestContains()
        {
            var rule = new MonthlyRecurrentTimeSlot(1, 31, new TimeSpan(0, 0, 0), new TimeSpan(23, 59, 00));
            Assert.IsTrue(rule.Contains(new System.DateTime(2015, 12, 1)));
            Assert.IsTrue(rule.Contains(new System.DateTime(2015, 12, 31)));
            Assert.IsTrue(rule.Contains(new System.DateTime(2015, 2, 28)));
        }

        [TestMethod]
        public void TestToString()
        {
            {
                var instance = new MonthlyRecurrentTimeSlot(16, 24);
                const string proof = "EVERY MONTH BETWEEN 16 AND 24";

                Assert.AreEqual(proof, instance.ToString());
            }

            {
                var instance = new MonthlyRecurrentTimeSlot(1, 12, new TimeSpan(8, 30, 00), new TimeSpan(9, 15, 00));
                const string proof = "EVERY MONTH BETWEEN 1 08:30 AND 12 09:15";

                Assert.AreEqual(proof, instance.ToString());
            }
        }
    }
}
