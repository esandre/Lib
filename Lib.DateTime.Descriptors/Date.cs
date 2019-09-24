using System;

namespace Lib.DateTime.Descriptors
{
    public class Date : IEquatable<Date>
    {
        private readonly System.DateTime _internal;

        public int Year => _internal.Year;
        public int Month => _internal.Month;
        public int Day => _internal.Day;

        public Date(int year, int month, int day)
        {
            _internal = new System.DateTime(year, month, day);
        }

        public static System.DateTime operator +(Date date, Time time)
        {
            return new System.DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute,
                time.Second, time.Millisecond);
        }

        public bool Equals(Date other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || _internal.Equals(other._internal);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Date) obj);
        }

        public override int GetHashCode()
        {
            return _internal.GetHashCode();
        }
    }

    public static class DateConverter
    {
        public static Date ToDate(this System.DateTime datetime)
        {
            return new Date(datetime.Year, datetime.Month, datetime.Day);
        }
    }
}
