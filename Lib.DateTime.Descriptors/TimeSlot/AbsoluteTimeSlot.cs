using System;
using System.Globalization;

namespace Lib.DateTime.Descriptors.TimeSlot
{
    public class AbsoluteTimeSlot : ITimeSlot
    {
        private readonly System.DateTime _beginning;
        private readonly System.DateTime _end;
        
        public AbsoluteTimeSlot(System.DateTime beginning, System.DateTime end)
        {
            if(beginning == end) throw new EmptyTimeSlotException();
            _beginning = beginning < end ? beginning : end;
            _end = end > beginning ? end : beginning;
        }

        public bool Contains(System.DateTime date)
        {
            return date >= _beginning && date <= _end;
        }

        public bool Outdated()
        {
            return System.DateTime.Now >= _end;
        }

        public bool IsAbsolute => true;

        public override string ToString()
        {
            return Descriptor.Keywords.Between + ' ' + _beginning.ToString("g") + ' ' + Descriptor.Keywords.And + ' ' + _end.ToString("g");
        }

        internal static AbsoluteTimeSlot FromDescriptor(string descriptor)
        {
            var splitted = descriptor.Split(' ', '/', ':');
            if (!splitted[0].Equals(Descriptor.Keywords.Between)) throw new InvalidDescriptorException(descriptor);
            switch (splitted.Length)
            {
                case 8:
                {
                    if (!splitted[4].Equals(Descriptor.Keywords.And)) throw new InvalidDescriptorException(descriptor);
                    var startDay = Convert.ToInt16(splitted[1]);
                    var startMonth = Convert.ToInt16(splitted[2]);
                    var startYear = Convert.ToInt16(splitted[3]);
                    var endDay = Convert.ToInt16(splitted[5]);
                    var endMonth = Convert.ToInt16(splitted[6]);
                    var endYear = Convert.ToInt16(splitted[7]);

                    return new AbsoluteTimeSlot(new System.DateTime(startYear, startMonth, startDay), new System.DateTime(endYear, endMonth, endDay, 23, 59, 59));
                }
                case 12:
                {
                    if (!splitted[6].Equals(Descriptor.Keywords.And)) throw new InvalidDescriptorException(descriptor);
                    var startDay = Convert.ToInt16(splitted[1]);
                    var startMonth = Convert.ToInt16(splitted[2]);
                    var startYear = Convert.ToInt16(splitted[3]);
                    var startHour = Convert.ToInt16(splitted[4]);
                    var startMinute = Convert.ToInt16(splitted[5]);

                    var endDay = Convert.ToInt16(splitted[7]);
                    var endMonth = Convert.ToInt16(splitted[8]);
                    var endYear = Convert.ToInt16(splitted[9]);
                    var endHour = Convert.ToInt16(splitted[10]);
                    var endMinute = Convert.ToInt16(splitted[11]);

                    return new AbsoluteTimeSlot(
                        new System.DateTime(startYear, startMonth, startDay, startHour, startMinute, 0), 
                        new System.DateTime(endYear, endMonth, endDay, endHour, endMinute, 0));
                }
                default : throw new InvalidDescriptorException(descriptor);
            }
        }

        public string ToString(CultureInfo culture)
        {
            return Descriptor.Translate(ToString(), CultureInfo.InvariantCulture, culture);
        }

        #region equality

        public bool Equals(AbsoluteTimeSlot other)
        {
            return ToString().Equals(other.ToString());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((AbsoluteTimeSlot) obj);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(AbsoluteTimeSlot left, AbsoluteTimeSlot right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AbsoluteTimeSlot left, AbsoluteTimeSlot right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
