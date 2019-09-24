using System.Collections.Generic;
using Lib.DateTime.Descriptors.TimeSlot;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.DateTime.Descriptors.Test
{
    [TestClass]
    public class TestAlias
    {
        private class TestAliasSource : IAliasProvider
        {
            private readonly IDictionary<string, ITimeSlot> _aliases;

            public TestAliasSource(IDictionary<string, ITimeSlot> aliases)
            {
                _aliases = aliases;
            }

            public ITimeSlot Fetch(string name)
            {
                return _aliases[name];
            }
        }

        [TestMethod]
        public void TestToString()
        {
            var aliases = new Dictionary<string, ITimeSlot> { {"saintglinglin", new AlwaysTimeSlot()} };
            const string descriptor = "EVERY DAY BETWEEN 10:00 AND 12:00 INTERSECTING :saintglinglin";
            var slot = new TimeSlotBuilder(new TestAliasSource(aliases)).Build(descriptor);
            Assert.AreEqual("EVERY DAY BETWEEN 10:00 AND 12:00 INTERSECTING :saintglinglin", slot.ToString());
        }

        [TestMethod]
        public void TestReplace()
        {
            var aliases = new Dictionary < string, ITimeSlot > { { "all", new AlwaysTimeSlot() } };
            const string descriptor = "NEVER OR :all";
            Assert.IsTrue(new TimeSlotBuilder(new TestAliasSource(aliases)).Build(descriptor).Contains(System.DateTime.Now));
        }

        [TestMethod]
        public void TestMultipleLayers()
        {
            var aliases = new Dictionary<string, ITimeSlot> {{"one", new NeverTimeSlot()}};
            aliases.Add("two", new TimeSlotBuilder(new TestAliasSource(aliases)).Build(":one OR NEVER"));
            aliases.Add("three", new TimeSlotBuilder(new TestAliasSource(aliases)).Build(":two OR NEVER"));
            aliases.Add("four", new TimeSlotBuilder(new TestAliasSource(aliases)).Build(":three OR NEVER"));
            aliases.Add("five", new TimeSlotBuilder(new TestAliasSource(aliases)).Build(":four OR ALWAYS"));

            const string descriptor = ":five OR NEVER";
            Assert.IsTrue(new TimeSlotBuilder(new TestAliasSource(aliases)).Build(descriptor).Contains(System.DateTime.Now));
        }
    }
}
