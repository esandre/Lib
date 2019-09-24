using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Lib.DateTime.Descriptors.TimeSlot
{
    public class WeeklyRecurrentTimeSlot : ITimeSlot, IEquatable<WeeklyRecurrentTimeSlot>
    {
        private readonly TimeSpan _startTimeFromMondayMidnight;
        private readonly TimeSpan _endTimeFromMondayMidnight;

        public bool IsAbsolute => false;

        public WeeklyRecurrentTimeSlot(DayOfWeek startDay, DayOfWeek endDay) :
            this(startDay, endDay, new TimeSpan(0), new TimeSpan(23, 59, 59))
        {
        }

        public WeeklyRecurrentTimeSlot(DayOfWeek startDay, DayOfWeek endDay, TimeSpan startTimeFromMidnight,
            TimeSpan endTimeFromMidnight)
            : this(
                startTimeFromMidnight + new TimeSpan(Frenchify(startDay), 0, 0, 0),
                endTimeFromMidnight + new TimeSpan(Frenchify(endDay), 0, 0, 0))
        {
        }

        private WeeklyRecurrentTimeSlot(TimeSpan startTimeFromMondayMidnight, TimeSpan endTimeFromMondayMidnight)
        {
            if (startTimeFromMondayMidnight == endTimeFromMondayMidnight) throw new EmptyTimeSlotException();
            if (startTimeFromMondayMidnight.TotalDays >= 7 || endTimeFromMondayMidnight.TotalDays > 7) throw new TooLongTimeSpanException();

            _startTimeFromMondayMidnight = startTimeFromMondayMidnight < endTimeFromMondayMidnight
                ? startTimeFromMondayMidnight
                : endTimeFromMondayMidnight;

            _endTimeFromMondayMidnight = endTimeFromMondayMidnight > startTimeFromMondayMidnight
                ? endTimeFromMondayMidnight
                : startTimeFromMondayMidnight;
        }

        public bool Contains(System.DateTime datetime)
        {
            var fromMondayDayOfWeek = Frenchify(datetime.DayOfWeek);
            var relative = new TimeSpan(fromMondayDayOfWeek, datetime.Hour, datetime.Minute, datetime.Second);
            return relative >= _startTimeFromMondayMidnight && relative <= _endTimeFromMondayMidnight;
        }

        internal static WeeklyRecurrentTimeSlot FromDescriptor(string descriptor)
        {
            var words = descriptor.Split(' ', ':');
            if (words[2] != Descriptor.Keywords.Between) throw new InvalidDescriptorException(descriptor);

            DayOfWeek startDow, endDow;
            int startHour, startMinutes, endHour, endMinutes;

            if (words.Length == 6 && words[4] == Descriptor.Keywords.And)
            {
                startDow = ReverseDays[words[3]];
                startHour = 0;
                startMinutes = 0;

                endDow = ReverseDays[words[5]];
                endHour = 23;
                endMinutes = 59;   
            } else if (words.Length == 10 && words[6] == Descriptor.Keywords.And) {
                startDow = ReverseDays[words[3]];
                startHour = int.Parse(words[4]);
                startMinutes = int.Parse(words[5]);

                endDow = ReverseDays[words[7]];
                endHour = int.Parse(words[8]);
                endMinutes = int.Parse(words[9]);
            } else throw new InvalidDescriptorException(descriptor);

            
            return new WeeklyRecurrentTimeSlot(startDow, endDow, 
                new TimeSpan(startHour, startMinutes, 0), 
                new TimeSpan(endHour, endMinutes, 59));
        }

        private static readonly string StringFormat = Descriptor.Keywords.Every + ' ' + Descriptor.Keywords.Week + ' ' +
                                                      Descriptor.Keywords.Between + " {0} {1}:{2} " + Descriptor.Keywords.And + " {3} {4}:{5}";

        public override string ToString()
        {
            return string.Format(StringFormat, 
                Days[_startTimeFromMondayMidnight.Days],
                _startTimeFromMondayMidnight.Hours.ToString("00"),
                _startTimeFromMondayMidnight.Minutes.ToString("00"),
                Days[_endTimeFromMondayMidnight.Days],
                _endTimeFromMondayMidnight.Hours.ToString("00"),
                _endTimeFromMondayMidnight.Minutes.ToString("00"));
        }

        public string ToString(CultureInfo culture)
        {
            return Descriptor.Translate(ToString(), CultureInfo.InvariantCulture, culture);
        }

        #region tables

        private readonly static Dictionary<string, DayOfWeek> ReverseDays = new Dictionary<string, DayOfWeek>
        {
            {Descriptor.Keywords.Monday, DayOfWeek.Monday},
            {Descriptor.Keywords.Tuesday, DayOfWeek.Tuesday},
            {Descriptor.Keywords.Wednesday, DayOfWeek.Wednesday},
            {Descriptor.Keywords.Thursday, DayOfWeek.Thursday},
            {Descriptor.Keywords.Friday, DayOfWeek.Friday},
            {Descriptor.Keywords.Saturday, DayOfWeek.Saturday},
            {Descriptor.Keywords.Sunday, DayOfWeek.Sunday}
        };

        private readonly static Dictionary<int, string> Days = new Dictionary<int, string>
        {
            {0, Descriptor.Keywords.Monday},
            {1, Descriptor.Keywords.Tuesday},
            {2, Descriptor.Keywords.Wednesday},
            {3, Descriptor.Keywords.Thursday},
            {4, Descriptor.Keywords.Friday},
            {5, Descriptor.Keywords.Saturday},
            {6, Descriptor.Keywords.Sunday}
        };

        private static int Frenchify(DayOfWeek day)
        {
            return Days.First(kv => kv.Value == ReverseDays.First(kv2 => kv2.Value == day).Key).Key;
        }

        #endregion

        #region equality

        public bool Equals(WeeklyRecurrentTimeSlot other)
        {
            return other != null && ToString().Equals(other.ToString());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((WeeklyRecurrentTimeSlot) obj);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(WeeklyRecurrentTimeSlot left, WeeklyRecurrentTimeSlot right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WeeklyRecurrentTimeSlot left, WeeklyRecurrentTimeSlot right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
