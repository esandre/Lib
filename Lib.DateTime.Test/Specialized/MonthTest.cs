using Lib.DateTime.Specialized;
using Lib.Random;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Lib.DateTime.Test.Specialized
{
    [TestClass]
    public class MonthTest : SpecializedDateTimeAbstractTest<Month>
    {
        [TestMethod]
        public void AMonth_IsADateTime_AtFirstDay_WithNoTime()
        {
            var dateTime = Random.NextDateTime();
            var month = new Month(dateTime.Year, dateTime.Month);

            System.DateTime monthAsDateTime = month;
            Check.That(monthAsDateTime.Year).Equals(dateTime.Year);
            Check.That(monthAsDateTime.Month).Equals(dateTime.Month);
            Check.That(monthAsDateTime.Day).Equals(1);
            Check.That(monthAsDateTime.Hour).Equals(0);
            Check.That(monthAsDateTime.Minute).Equals(0);
            Check.That(monthAsDateTime.Second).Equals(0);
            Check.That(monthAsDateTime.Millisecond).Equals(0);
        }
    }
}
