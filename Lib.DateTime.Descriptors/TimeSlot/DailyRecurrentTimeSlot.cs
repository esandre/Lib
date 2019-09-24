using System;
using System.Globalization;

namespace Lib.DateTime.Descriptors.TimeSlot
{
    public class DailyRecurrentTimeSlot : ITimeSlot, IEquatable<DailyRecurrentTimeSlot>
    {
        private readonly TimeSpan _startTimeFromMidnight;
        private readonly TimeSpan _endTimeFromMidnight;

        public DailyRecurrentTimeSlot(TimeSpan startTimeFromMidnight, TimeSpan endTimeFromMidnight)
        {
            if (startTimeFromMidnight == endTimeFromMidnight) throw new EmptyTimeSlotException();
            if (startTimeFromMidnight.TotalDays >= 1 || endTimeFromMidnight.TotalDays > 1) throw new TooLongTimeSpanException();

            _startTimeFromMidnight = startTimeFromMidnight < endTimeFromMidnight
                ? startTimeFromMidnight
                : endTimeFromMidnight;

            _endTimeFromMidnight = endTimeFromMidnight > startTimeFromMidnight
                ? endTimeFromMidnight
                : startTimeFromMidnight;
        }

        public bool Contains(System.DateTime datetime)
        {
            var relative = new TimeSpan(datetime.Hour, datetime.Minute, datetime.Second);
            return relative >= _startTimeFromMidnight && relative <= _endTimeFromMidnight;
        }

        public bool IsAbsolute => false;

        internal static DailyRecurrentTimeSlot FromDescriptor(string descriptor)
        {
            var words = descriptor.Split(' ', ':');
            if (words[0] != Descriptor.Keywords.Every 
                || words[1] != Descriptor.Keywords.Day 
                || words[2] != Descriptor.Keywords.Between 
                || words.Length != 8 
                || words[5] != Descriptor.Keywords.And) throw new InvalidDescriptorException(descriptor);

            var startHour = int.Parse(words[3]);
            var startMinutes = int.Parse(words[4]);
            var endHours = int.Parse(words[6]);
            var endMinutes = int.Parse(words[7]);

            return new DailyRecurrentTimeSlot(new TimeSpan(startHour, startMinutes, 0), new TimeSpan(endHours, endMinutes, 0));
        }

        private static readonly string StringFormat =
            Descriptor.Keywords.Every + ' ' + Descriptor.Keywords.Day + ' ' +
            Descriptor.Keywords.Between + " {0}:{1} " +
            Descriptor.Keywords.And + " {2}:{3}";

        public override string ToString()
        {
            return string.Format(StringFormat, 
                _startTimeFromMidnight.Hours.ToString("00"),
                _startTimeFromMidnight.Minutes.ToString("00"), 
                _endTimeFromMidnight.Hours.ToString("00"), 
                _endTimeFromMidnight.Minutes.ToString("00"));
        }

        public string ToString(CultureInfo culture)
        {
            return Descriptor.Translate(ToString(), CultureInfo.InvariantCulture, culture);
        }

        #region equality

        public bool Equals(DailyRecurrentTimeSlot other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _startTimeFromMidnight.Equals(other._startTimeFromMidnight) && _endTimeFromMidnight.Equals(other._endTimeFromMidnight);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((DailyRecurrentTimeSlot) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_startTimeFromMidnight.GetHashCode()*397) ^ _endTimeFromMidnight.GetHashCode();
            }
        }

        public static bool operator ==(DailyRecurrentTimeSlot left, DailyRecurrentTimeSlot right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DailyRecurrentTimeSlot left, DailyRecurrentTimeSlot right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
