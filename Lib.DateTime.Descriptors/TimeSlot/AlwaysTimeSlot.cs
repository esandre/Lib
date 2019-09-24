using System.Globalization;

namespace Lib.DateTime.Descriptors.TimeSlot
{
    public class AlwaysTimeSlot : ITimeSlot
    {
        public bool Contains(System.DateTime date)
        {
            return true;
        }

        public bool IsAbsolute { get { return false; } }

        public override string ToString()
        {
            return Descriptor.Keywords.Always;
        }

        public string ToString(CultureInfo culture)
        {
            return Descriptor.Translate(ToString(), CultureInfo.InvariantCulture, culture);
        }
    }
}
