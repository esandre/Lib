using System.Globalization;

namespace Lib.DateTime.Descriptors.TimeSlot
{
    public class NeverTimeSlot : ITimeSlot
    {
        public bool Contains(System.DateTime date)
        {
            return false;
        }

        public bool IsAbsolute { get { return false; } }

        public override string ToString()
        {
            return Descriptor.Keywords.Never;
        }

        public string ToString(CultureInfo culture)
        {
            return Descriptor.Translate(ToString(), CultureInfo.InvariantCulture, culture);
        }
    }
}
