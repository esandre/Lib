using System;
using Lib.DateTime.Descriptors.TimeSlot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lib.DateTime.Descriptors.Test
{
    [TestClass]
    public class TestDailyRecurrenceRule
    {
        private static readonly IAliasProvider NoAliasProvider = Mock.Of<IAliasProvider>(MockBehavior.Strict);

        [TestMethod]
        public void TestBuilder()
        {
            var proof = new DailyRecurrentTimeSlot(new TimeSpan(8, 30, 00), new TimeSpan(9, 15, 00));
            const string str = "EVERY DAY BETWEEN 08:30 AND 09:15";
            var builded = new TimeSlotBuilder(NoAliasProvider).Build(str);

            Assert.AreEqual(proof, builded);
        }

        [TestMethod]
        public void TestContains()
        {
            {
                var rule = new DailyRecurrentTimeSlot(TimeSpan.Zero, new TimeSpan(23, 59, 59));
                Assert.IsTrue(rule.Contains(System.DateTime.Now));
            }

            {
                var rule = new DailyRecurrentTimeSlot(new TimeSpan(12, 36, 59), new TimeSpan(23, 59, 59));
                Assert.IsFalse(rule.Contains(new System.DateTime(1934, 2, 6, 12, 35, 45)));
            }

            {
                var rule = new DailyRecurrentTimeSlot(new TimeSpan(12, 34, 59), new TimeSpan(23, 59, 59));
                Assert.IsTrue(rule.Contains(new System.DateTime(1934, 2, 6, 12, 35, 45)));
            }
        }

        [TestMethod]
        public void TestToString()
        {
            {
                var instance = new DailyRecurrentTimeSlot(TimeSpan.Zero, new TimeSpan(23, 59, 59));
                const string proof = "EVERY DAY BETWEEN 00:00 AND 23:59";

                Assert.AreEqual(proof, instance.ToString());
            }

            {
                var instance = new DailyRecurrentTimeSlot(new TimeSpan(8, 30, 00), new TimeSpan(9, 15, 00));
                const string proof = "EVERY DAY BETWEEN 08:30 AND 09:15";

                Assert.AreEqual(proof, instance.ToString());
            }
        }
    }
}
