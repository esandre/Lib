using System.Globalization;
using Lib.DateTime.Descriptors.TimeSlot;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.DateTime.Descriptors.Test
{
    [TestClass]
    public class TestComplexTimeSlot
    {
        [TestMethod]
        public void TestContains()
        {
            {
                var rule = ComplexTimeSlot.CreateOr(new AlwaysTimeSlot(), new NeverTimeSlot());
                Assert.IsTrue(rule.Contains(System.DateTime.Now));
            }

            {
                var rule = ComplexTimeSlot.CreateOr(new NeverTimeSlot(), new NeverTimeSlot());
                Assert.IsFalse(rule.Contains(System.DateTime.Now));
            }

            {
                var rule = ComplexTimeSlot.CreateOr(new AlwaysTimeSlot(), new AlwaysTimeSlot());
                Assert.IsTrue(rule.Contains(System.DateTime.Now));
            }

            {
                var rule = ComplexTimeSlot.CreateIntersecting(new AlwaysTimeSlot(), new NeverTimeSlot());
                Assert.IsFalse(rule.Contains(System.DateTime.Now));
            }

            {
                var rule = ComplexTimeSlot.CreateIntersecting(new NeverTimeSlot(), new NeverTimeSlot());
                Assert.IsFalse(rule.Contains(System.DateTime.Now));
            }

            {
                var rule = ComplexTimeSlot.CreateIntersecting(new AlwaysTimeSlot(), new AlwaysTimeSlot());
                Assert.IsTrue(rule.Contains(System.DateTime.Now));
            }

            {
                var rule = ComplexTimeSlot.CreateBut(new AlwaysTimeSlot(), new NeverTimeSlot());
                Assert.IsTrue(rule.Contains(System.DateTime.Now));
            }

            {
                var rule = ComplexTimeSlot.CreateBut(new NeverTimeSlot(), new NeverTimeSlot());
                Assert.IsFalse(rule.Contains(System.DateTime.Now));
            }

            {
                var rule = ComplexTimeSlot.CreateBut(new AlwaysTimeSlot(), new AlwaysTimeSlot());
                Assert.IsFalse(rule.Contains(System.DateTime.Now));
            }
        }

        [TestMethod]
        public void TestDescriptor()
        {
            {
                var proof = ComplexTimeSlot.CreateOr(new AbsoluteTimeSlot(new System.DateTime(2015, 06, 12, 12, 21, 55), new System.DateTime(2015, 07, 03, 17, 4, 00)), new NeverTimeSlot());
                const string descriptor = "BETWEEN 12/06/2015 12:21 AND 03/07/2015 17:04 OR NEVER";
                Assert.AreEqual((object) proof, new TimeSlotBuilder().Build(descriptor));
            }

            {
                var proof = ComplexTimeSlot.CreateIntersecting(
                    ComplexTimeSlot.CreateOr(
                        new AlwaysTimeSlot(),
                        ComplexTimeSlot.CreateOr(
                            ComplexTimeSlot.CreateBut(new NeverTimeSlot(), new AlwaysTimeSlot()),
                            new NeverTimeSlot())),
                    new NeverTimeSlot());

                const string descriptor = "(ALWAYS OR ((NEVER BUT ALWAYS) OR NEVER)) INTERSECTING NEVER";
                Assert.AreEqual((object) proof, new TimeSlotBuilder().Build(descriptor));
            }
        }

        [TestMethod]
        public void TestFrench()
        {
            {
                var proof = ComplexTimeSlot.CreateOr(new AbsoluteTimeSlot(new System.DateTime(2015, 06, 12, 12, 21, 55), new System.DateTime(2015, 07, 03, 17, 4, 00)), new NeverTimeSlot());
                const string descriptor = "ENTRE 12/06/2015 12:21 ET 03/07/2015 17:04 OU JAMAIS";
                Assert.AreEqual((object) proof, new TimeSlotBuilder(new CultureInfo("fr")).Build(descriptor));
            }

            {
                var proof = ComplexTimeSlot.CreateIntersecting(
                    ComplexTimeSlot.CreateOr(
                        new AlwaysTimeSlot(),
                        ComplexTimeSlot.CreateOr(
                            ComplexTimeSlot.CreateBut(new NeverTimeSlot(), new AlwaysTimeSlot()),
                            new NeverTimeSlot())),
                    new NeverTimeSlot());

                const string descriptor = "(TOUJOURS OU ((JAMAIS SAUF TOUJOURS) OU JAMAIS)) RECOUPANT JAMAIS";
                Assert.AreEqual((object) proof, new TimeSlotBuilder(new CultureInfo("fr")).Build(descriptor));
            }
        }
    }
}
