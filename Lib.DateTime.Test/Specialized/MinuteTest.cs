using Lib.DateTime.Specialized;
using Lib.Random;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Lib.DateTime.Test.Specialized
{
    [TestClass]
    public class MinuteTest : SpecializedDateTimeAbstractTest
    {
        [TestMethod]
        public void AMinute_IsADateTime_At00s()
        {
            var dateTime = Random.NextDateTime();
            var minute = new Minute(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute);

            System.DateTime minuteAsDateTime = minute;
            Check.That(minuteAsDateTime.Year).Equals(dateTime.Year);
            Check.That(minuteAsDateTime.Month).Equals(dateTime.Month);
            Check.That(minuteAsDateTime.Day).Equals(dateTime.Day);
            Check.That(minuteAsDateTime.Hour).Equals(dateTime.Hour);
            Check.That(minuteAsDateTime.Minute).Equals(dateTime.Minute);
            Check.That(minuteAsDateTime.Second).Equals(0);
            Check.That(minuteAsDateTime.Millisecond).Equals(0);
        }
    }
}
