using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Lib.DateTime.Descriptors.TimeSlot
{
    public class AliasTimeSlot : ITimeSlot
    {
        private readonly string _nom;
        private readonly ITimeSlot _behind;

        private AliasTimeSlot(string name, IAliasProvider provider)
        {
            if(!Regex.IsMatch(name, "^[a-zA-Z0-9]+$")) throw new Exception("Name can only contain letters A-Z or a-z or numbers");
            _nom = name;
            _behind = provider.Fetch(_nom);
        }

        public bool Contains(System.DateTime date)
        {
            return _behind.Contains(date);
        }

        public bool IsAbsolute => _behind.IsAbsolute;

        public override string ToString()
        {
            return ":" + _nom;
        }

        public string ToString(CultureInfo culture)
        {
            return ToString();
        }

        public static AliasTimeSlot FromDescriptor(string descriptor, IAliasProvider provider)
        {
            if(!descriptor.StartsWith(":")) throw new Exception("Not an alias descriptor");
            return new AliasTimeSlot(descriptor.TrimStart(':'), provider);
        }
    }
}
