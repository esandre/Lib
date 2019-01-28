using Lib.DateTime.Specialized;
using Lib.Random;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Lib.DateTime.Test.Specialized
{
    [TestClass]
    public class DayTest : SpecializedDateTimeAbstractTest<Day>
    {
        [TestMethod]
        public void ADay_IsADateTime_AtMidnight()
        {
            var dateTime = Random.NextDateTime();
            var day = new Day(dateTime.Year, dateTime.Month, dateTime.Day);

            System.DateTime dayAsDateTime = day;
            Check.That(dayAsDateTime.Year).Equals(dateTime.Year);
            Check.That(dayAsDateTime.Month).Equals(dateTime.Month);
            Check.That(dayAsDateTime.Day).Equals(dateTime.Day);
            Check.That(dayAsDateTime.Hour).Equals(0);
            Check.That(dayAsDateTime.Minute).Equals(0);
            Check.That(dayAsDateTime.Second).Equals(0);
            Check.That(dayAsDateTime.Millisecond).Equals(0);
        }
    }
}
