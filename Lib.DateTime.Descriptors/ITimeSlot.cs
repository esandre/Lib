using System.Globalization;

namespace Lib.DateTime.Descriptors
{
    public interface ITimeSlot
    {
        bool Contains(System.DateTime date);
        bool IsAbsolute { get; }
        string ToString(CultureInfo culture);
    }
}
