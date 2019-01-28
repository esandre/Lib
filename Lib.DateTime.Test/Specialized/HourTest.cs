using Lib.DateTime.Specialized;
using Lib.Random;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Lib.DateTime.Test.Specialized
{
    [TestClass]
    public class HourTest : SpecializedDateTimeAbstractTest<Hour>
    {
        [TestMethod]
        public void AnHour_IsADateTime_At00_00()
        {
            var dateTime = Random.NextDateTime();
            var hour = new Hour(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour);

            System.DateTime hourAsDateTime = hour;
            Check.That(hourAsDateTime.Year).Equals(dateTime.Year);
            Check.That(hourAsDateTime.Month).Equals(dateTime.Month);
            Check.That(hourAsDateTime.Day).Equals(dateTime.Day);
            Check.That(hourAsDateTime.Hour).Equals(dateTime.Hour);
            Check.That(hourAsDateTime.Minute).Equals(0);
            Check.That(hourAsDateTime.Second).Equals(0);
            Check.That(hourAsDateTime.Millisecond).Equals(0);
        }
    }
}
