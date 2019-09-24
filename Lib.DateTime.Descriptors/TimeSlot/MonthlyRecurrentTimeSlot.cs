using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Lib.DateTime.Descriptors.TimeSlot
{
    public class MonthlyRecurrentTimeSlot : ITimeSlot, IEquatable<MonthlyRecurrentTimeSlot>
    {
        private readonly TimeSpan _startTimeFromFirstDayMidnight;
        private readonly TimeSpan _endTimeFromFirstDayMidnight;
        private readonly bool _timeSpecified;

        public bool IsAbsolute => false;

        public MonthlyRecurrentTimeSlot(ushort startDayOfMonth, ushort endDayOfMonth,
            TimeSpan startTimeFromMidnight, TimeSpan endTimeFromMidnight)
            : this(startTimeFromMidnight.Add(new TimeSpan(startDayOfMonth, 0, 0, 0)),
            endTimeFromMidnight.Add(new TimeSpan(endDayOfMonth, 0, 0, 0)))
        {
            _timeSpecified = true;
        }

        public MonthlyRecurrentTimeSlot(ushort startDayOfMonth, ushort endDayOfMonth)
            : this(new TimeSpan(startDayOfMonth, 0, 0, 0), new TimeSpan(endDayOfMonth, 23, 59, 59))
        {
            _timeSpecified = false;
        }

        private MonthlyRecurrentTimeSlot(TimeSpan startTimeFromFirstDayMidnight, TimeSpan endTimeFromFirstDayMidnight)
        {
            if (startTimeFromFirstDayMidnight == endTimeFromFirstDayMidnight) throw new EmptyTimeSlotException();
            if (startTimeFromFirstDayMidnight.TotalDays >= 31 || endTimeFromFirstDayMidnight.TotalDays >= 32) throw new TooLongTimeSpanException();

            _startTimeFromFirstDayMidnight = startTimeFromFirstDayMidnight < endTimeFromFirstDayMidnight
                ? startTimeFromFirstDayMidnight
                : endTimeFromFirstDayMidnight;

            _endTimeFromFirstDayMidnight = endTimeFromFirstDayMidnight > startTimeFromFirstDayMidnight
                ? endTimeFromFirstDayMidnight
                : startTimeFromFirstDayMidnight;
        }

        public bool Contains(System.DateTime datetime)
        {
            System.DateTime start;
            try
            {
                start = new System.DateTime(datetime.Year, datetime.Month, 1).AddDays(-1) + _startTimeFromFirstDayMidnight;
            }
            catch (ArgumentOutOfRangeException)
            {
                start = new System.DateTime(datetime.Year, datetime.Month, 1, 0, 0, 0);
            }

            System.DateTime end;
            try
            {
                end = new System.DateTime(datetime.Year, datetime.Month, 1).AddDays(-1) + _endTimeFromFirstDayMidnight;
            }
            catch (ArgumentOutOfRangeException)
            {
                end = new System.DateTime(datetime.Year, datetime.Month, 1).AddMonths(1).AddSeconds(-1);
            }
                
            return datetime >= start && datetime <= end;
        }

        internal static MonthlyRecurrentTimeSlot FromDescriptor(string descriptor)
        {
            var words = descriptor.Split(' ', ':', '/');
            if (words[2] != Descriptor.Keywords.Between) throw new InvalidDescriptorException(descriptor);

            switch (words.Length)
            {
                case 10:
                    return FromLongDescriptor(words);
                case 6:
                    return FromShortDescriptor(words);
                default:
                    throw new InvalidDescriptorException(descriptor);
            }
        }

        private static MonthlyRecurrentTimeSlot FromShortDescriptor(IList<string> words)
        {
            if (words[4] != Descriptor.Keywords.And) throw new InvalidDescriptorException(words.Aggregate((c, e) => c + ' ' + e));

            var startDay = ushort.Parse(words[3]);
            var endDay = ushort.Parse(words[5]);

            return new MonthlyRecurrentTimeSlot(startDay, endDay);
        }

        private static MonthlyRecurrentTimeSlot FromLongDescriptor(IList<string> words)
        {
            if (words[6] != Descriptor.Keywords.And) throw new InvalidDescriptorException(words.Aggregate((c, e) => c + ' ' + e));

            var startDay = ushort.Parse(words[3]);
            var startHour = int.Parse(words[4]);
            var startMinutes = int.Parse(words[5]);

            var endDay = ushort.Parse(words[7]);
            var endHours = int.Parse(words[8]);
            var endMinutes = int.Parse(words[9]);

            return new MonthlyRecurrentTimeSlot(startDay, endDay, new TimeSpan(startHour, startMinutes, 0), new TimeSpan(endHours, endMinutes, 0));
        }

        public override string ToString()
        {
            var output = Descriptor.Keywords.Every + ' ' + Descriptor.Keywords.Month + ' ' + Descriptor.Keywords.Between + ' ' + _startTimeFromFirstDayMidnight.Days;

            if (_timeSpecified)
            {
                var time = new System.DateTime(1, 1, 1, _startTimeFromFirstDayMidnight.Hours, _startTimeFromFirstDayMidnight.Minutes, 0);
                output += ' ' + time.ToString("t");
            }

            output += ' ' + Descriptor.Keywords.And + ' ' + _endTimeFromFirstDayMidnight.Days;

            if (_timeSpecified)
            {
                var time = new System.DateTime(1, 1, 1, _endTimeFromFirstDayMidnight.Hours, _endTimeFromFirstDayMidnight.Minutes, 0);
                output += ' ' + time.ToString("t");
            }

            return output;
        }

        public string ToString(CultureInfo culture)
        {
            return Descriptor.Translate(ToString(), CultureInfo.InvariantCulture, culture);
        }

        #region equality

        public bool Equals(MonthlyRecurrentTimeSlot other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _startTimeFromFirstDayMidnight.Equals(other._startTimeFromFirstDayMidnight) && _endTimeFromFirstDayMidnight.Equals(other._endTimeFromFirstDayMidnight);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((MonthlyRecurrentTimeSlot) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_startTimeFromFirstDayMidnight.GetHashCode()*397) ^ _endTimeFromFirstDayMidnight.GetHashCode();
            }
        }

        public static bool operator ==(MonthlyRecurrentTimeSlot left, MonthlyRecurrentTimeSlot right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MonthlyRecurrentTimeSlot left, MonthlyRecurrentTimeSlot right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
