using System.Globalization;
using System.Linq;
using Lib.DateTime.Descriptors.TimeSlot;

namespace Lib.DateTime.Descriptors
{
    public class TimeSlotBuilder
    {
        private readonly CultureInfo _culture;
        private readonly IAliasProvider _aliasesProvider;

        public TimeSlotBuilder(IAliasProvider aliases) :
            this(aliases, CultureInfo.InvariantCulture)
        {
        }

        public TimeSlotBuilder(IAliasProvider aliases, CultureInfo culture)
        {
            _aliasesProvider = aliases;
            _culture = culture;
        }

        public ITimeSlot Build(string descriptor)
        {
            descriptor = Descriptor.Translate(descriptor, _culture, CultureInfo.InvariantCulture);

            if (descriptor == Descriptor.Keywords.Always) return new AlwaysTimeSlot();
            if (descriptor == Descriptor.Keywords.Never) return new NeverTimeSlot();

            var words = descriptor.Split(' ').ToList();

            if (words.Contains(Descriptor.Keywords.Intersecting)
                || words.Contains(Descriptor.Keywords.Or)
                || words.Contains(Descriptor.Keywords.But)
                || descriptor.Contains("(")
                || descriptor.Contains(")")) return ComplexTimeSlot.FromDescriptor(descriptor, _aliasesProvider);
            
            if (descriptor.StartsWith(Descriptor.Keywords.Between)) return AbsoluteTimeSlot.FromDescriptor(descriptor);
            if (descriptor.StartsWith(":")) return AliasTimeSlot.FromDescriptor(descriptor, _aliasesProvider);
            if (!descriptor.StartsWith(Descriptor.Keywords.Every)) throw new InvalidDescriptorException(descriptor);

            if (words.Contains(Descriptor.Keywords.Day)) return DailyRecurrentTimeSlot.FromDescriptor(descriptor);
            if (words.Contains(Descriptor.Keywords.Month)) return MonthlyRecurrentTimeSlot.FromDescriptor(descriptor);
            if (words.Contains(Descriptor.Keywords.Week)) return WeeklyRecurrentTimeSlot.FromDescriptor(descriptor);
            if (words.Contains(Descriptor.Keywords.Year)) return YearlyRecurrentTimeSlot.FromDescriptor(descriptor);

            throw new InvalidDescriptorException(descriptor);
        }
    }
}
