using Lib.DateTime.Specialized;
using Lib.Random;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Lib.DateTime.Test.Specialized
{
    [TestClass]
    public class YearTest : SpecializedDateTimeAbstractTest
    {
        [TestMethod]
        public void AYear_IsADateTime_AtFirstMonthAndDay_WithNoTime()
        {
            var dateTime = Random.NextDateTime();
            var year = new Year(dateTime.Year);

            System.DateTime secondAsDateTime = year;
            Check.That(secondAsDateTime.Year).Equals(dateTime.Year);
            Check.That(secondAsDateTime.Month).Equals(1);
            Check.That(secondAsDateTime.Day).Equals(1);
            Check.That(secondAsDateTime.Hour).Equals(0);
            Check.That(secondAsDateTime.Minute).Equals(0);
            Check.That(secondAsDateTime.Second).Equals(0);
            Check.That(secondAsDateTime.Millisecond).Equals(0);
        }
    }
}
