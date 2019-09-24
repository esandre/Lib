using System;
using System.Collections.Generic;
using System.Globalization;

namespace Lib.DateTime.Descriptors.TimeSlot
{
    public class YearlyRecurrentTimeSlot : ITimeSlot, IEquatable<YearlyRecurrentTimeSlot>
    {
        private readonly System.DateTime _startTime;
        private readonly System.DateTime _endTime;

        public bool IsAbsolute => false;

        public YearlyRecurrentTimeSlot(int startDay, int startMonth, int endDay, int endMonth)
            : this(startDay, startMonth, new TimeSpan(0), endDay, endMonth, new TimeSpan(0, 23, 59, 0))
        {
        }

        public YearlyRecurrentTimeSlot(int startDay, int startMonth, TimeSpan startTime, int endDay, int endMonth, TimeSpan endTime)
            : this(new System.DateTime(System.DateTime.Now.Year, startMonth, startDay, startTime.Hours, startTime.Minutes, startTime.Seconds),
                  new System.DateTime(System.DateTime.Now.Year, endMonth, endDay, endTime.Hours, endTime.Minutes, endTime.Seconds))
        {
        }

        private YearlyRecurrentTimeSlot(System.DateTime start, System.DateTime end)
        {
            if (start == end) throw new EmptyTimeSlotException();

            _startTime = start < end
                ? start
                : end;

            _endTime = end > start
                ? end
                : start;
        }

        public bool Contains(System.DateTime datetime)
        {
            var start = new System.DateTime(datetime.Year, _startTime.Month, _startTime.Day, _startTime.Hour, _startTime.Minute, _startTime.Second);
            var end = new System.DateTime(datetime.Year, _endTime.Month, _endTime.Day, _endTime.Hour, _endTime.Minute, _endTime.Second);
            return datetime >= start && datetime <= end;
        }

        private static YearlyRecurrentTimeSlot FromShortDescriptor(string[] words)
        {
            if (words[5] != Descriptor.Keywords.And) throw new InvalidDescriptorException(null);

            var startMonth = int.Parse(words[4]);
            var startDay = int.Parse(words[3]);

            var endMonth = int.Parse(words[7]);
            var endDay = int.Parse(words[6]);

            return new YearlyRecurrentTimeSlot(
                new System.DateTime(1, startMonth, startDay, 0, 0, 0),
                new System.DateTime(1, endMonth, endDay, 23, 59, 59));
        }

        private static YearlyRecurrentTimeSlot FromLongDescriptor(IList<string> words)
        {
            if (words[7] != Descriptor.Keywords.And) throw new InvalidDescriptorException(null);

            var startMonth = int.Parse(words[4]);
            var startDay = int.Parse(words[3]);
            var startHour = int.Parse(words[5]);
            var startMinutes = int.Parse(words[6]);

            var endMonth = int.Parse(words[9]);
            var endDay = int.Parse(words[8]);
            var endHours = int.Parse(words[10]);
            var endMinutes = int.Parse(words[11]);

            return new YearlyRecurrentTimeSlot(
                new System.DateTime(1, startMonth, startDay, startHour, startMinutes, 0),
                new System.DateTime(1, endMonth, endDay, endHours, endMinutes, 59));
        }

        internal static YearlyRecurrentTimeSlot FromDescriptor(string descriptor)
        {
            try
            {
                var words = descriptor.Split(' ', ':', '/');
                if (words[2] != Descriptor.Keywords.Between) throw new InvalidDescriptorException(null);

                switch (words.Length)
                {
                    case 12:
                        return FromLongDescriptor(words);
                    case 8:
                    {
                        return FromShortDescriptor(words);
                    }
                    default:
                        throw new InvalidDescriptorException(null);
                }
            }
            catch (InvalidDescriptorException)
            {
                throw new InvalidDescriptorException(descriptor);
            }
        }

        public override string ToString()
        {
            return Descriptor.Keywords.Every + ' ' + Descriptor.Keywords.Year + ' ' + Descriptor.Keywords.Between + ' '
                + _startTime.ToString("dd/MM HH:mm") + ' ' + Descriptor.Keywords.And + ' ' +
                   _endTime.ToString("dd/MM HH:mm");
        }

        public string ToString(CultureInfo culture)
        {
            return Descriptor.Translate(ToString(), CultureInfo.InvariantCulture, culture);
        }

        #region equality

        public bool Equals(YearlyRecurrentTimeSlot other)
        {
            return other != null && ToString().Equals(other.ToString());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((YearlyRecurrentTimeSlot) obj);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(YearlyRecurrentTimeSlot left, YearlyRecurrentTimeSlot right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(YearlyRecurrentTimeSlot left, YearlyRecurrentTimeSlot right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
