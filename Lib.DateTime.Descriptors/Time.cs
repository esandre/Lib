using System;

namespace Lib.DateTime.Descriptors
{
    public class Time
    {
        private readonly TimeSpan _internal;

        public int Hour => _internal.Hours;
        public int Minute => _internal.Minutes;
        public int Second => _internal.Seconds;
        public int Millisecond => _internal.Milliseconds;

        public Time(int hour, int minute, int second = 0, int millisecond = 0)
        {
            _internal = new TimeSpan(0, hour, minute, second, millisecond);
        }

        public static System.DateTime operator +(Time time, Date date)
        {
            return new System.DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute,
                time.Second, time.Millisecond);
        }

        private bool Equals(Time other)
        {
            return _internal.Equals(other._internal);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Time) obj);
        }

        public override int GetHashCode()
        {
            return _internal.GetHashCode();
        }
    } 

    public static class TimeConverter
    {
        public static Time ToTime(this System.DateTime datetime)
        {
            return new Time(datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond);
        }
    }
}
