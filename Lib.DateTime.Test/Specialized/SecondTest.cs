using Lib.DateTime.Specialized;
using Lib.Random;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Lib.DateTime.Test.Specialized
{
    [TestClass]
    public class SecondTest : SpecializedDateTimeAbstractTest
    {
        [TestMethod]
        public void ASecond_IsADateTime_WithNoMilliseconds()
        {
            var dateTime = Random.NextDateTime();
            var second = new Second(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

            System.DateTime secondAsDateTime = second;
            Check.That(secondAsDateTime.Year).Equals(dateTime.Year);
            Check.That(secondAsDateTime.Month).Equals(dateTime.Month);
            Check.That(secondAsDateTime.Day).Equals(dateTime.Day);
            Check.That(secondAsDateTime.Hour).Equals(dateTime.Hour);
            Check.That(secondAsDateTime.Minute).Equals(dateTime.Minute);
            Check.That(secondAsDateTime.Second).Equals(dateTime.Second);
            Check.That(secondAsDateTime.Millisecond).Equals(0);
        }
    }
}
