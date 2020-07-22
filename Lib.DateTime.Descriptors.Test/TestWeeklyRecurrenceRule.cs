using System;
using Lib.DateTime.Descriptors.TimeSlot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lib.DateTime.Descriptors.Test
{
    [TestClass]
    public class TestWeeklyRecurrenceRule
    {
        private static readonly IAliasProvider NoAliasProvider = Mock.Of<IAliasProvider>(MockBehavior.Strict);
        
        [TestMethod]
        public void TestBuilder()
        {
            {
                var proof = new WeeklyRecurrentTimeSlot(DayOfWeek.Monday, DayOfWeek.Thursday);
                const string str = "EVERY WEEK BETWEEN MONDAY 00:00 AND THURSDAY 23:59";
                var builded = new TimeSlotBuilder(NoAliasProvider).Build(str);

                Assert.AreEqual(proof, builded);
            }

            {
                var proof = new WeeklyRecurrentTimeSlot(DayOfWeek.Monday, DayOfWeek.Thursday, new TimeSpan(8, 30, 00), new TimeSpan(9, 15, 00));
                const string str = "EVERY WEEK BETWEEN MONDAY 08:30 AND THURSDAY 09:15";
                var builded = new TimeSlotBuilder(NoAliasProvider).Build(str);

                Assert.AreEqual(proof, builded);
            }
        }

        [TestMethod]
        public void TestContains()
        {
            {
                var rule = new WeeklyRecurrentTimeSlot(DayOfWeek.Monday, DayOfWeek.Sunday);
                Assert.IsTrue(rule.Contains(System.DateTime.Now));
            }

            {
                var rule = new WeeklyRecurrentTimeSlot(DayOfWeek.Monday, DayOfWeek.Sunday, new TimeSpan(12, 36, 59), new TimeSpan(23, 59, 59));
                Assert.IsFalse(rule.Contains(new System.DateTime(2014, 8, 4, 12, 35, 45)));
            }

            {
                var rule = new WeeklyRecurrentTimeSlot(DayOfWeek.Monday, DayOfWeek.Sunday, new TimeSpan(12, 34, 59), new TimeSpan(23, 59, 59));
                Assert.IsTrue(rule.Contains(new System.DateTime(1934, 2, 6, 12, 35, 45)));
            }

            {
                var rule = new WeeklyRecurrentTimeSlot(DayOfWeek.Monday, DayOfWeek.Sunday);
                Assert.IsTrue(rule.Contains(new System.DateTime(2015, 4, 26, 23, 59, 59)));
            }
        }

        [TestMethod]
        public void TestToString()
        {
            {
                var proof = new WeeklyRecurrentTimeSlot(DayOfWeek.Monday, DayOfWeek.Thursday);
                const string str = "EVERY WEEK BETWEEN MONDAY 00:00 AND THURSDAY 23:59";

                Assert.AreEqual(str, proof.ToString());
            }

            {
                var proof = new WeeklyRecurrentTimeSlot(DayOfWeek.Monday, DayOfWeek.Thursday, new TimeSpan(8, 30, 00), new TimeSpan(9, 15, 00));
                const string str = "EVERY WEEK BETWEEN MONDAY 08:30 AND THURSDAY 09:15";

                Assert.AreEqual(str, proof.ToString());
            }
        }
    }
}
