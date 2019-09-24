using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Lib.DateTime.Descriptors
{
    public static class Descriptor
    {
        public class DescriptorKeywords
        {
            public readonly string And;
            public readonly string Or;
            public readonly string But;
            public readonly string Intersecting;
            public readonly string Between;
            public readonly string Every;
            public readonly string Day;
            public readonly string Month;
            public readonly string Week;
            public readonly string Year;
            public readonly string Always;
            public readonly string Never;
            public readonly string Monday;
            public readonly string Tuesday;
            public readonly string Wednesday;
            public readonly string Thursday;
            public readonly string Friday;
            public readonly string Saturday;
            public readonly string Sunday;

            public DescriptorKeywords(string and, string or, string but, string intersecting, string between,
                string every, string day, string month, string week, string year, string always, string never,
                string monday, string tuesday, string wednesday, string thursday, string friday, string saturday,
                string sunday)
            {
                And = and;
                Or = or;
                But = but;
                Intersecting = intersecting;
                Between = between;
                Every = every;
                Day = day;
                Month = month;
                Week = week;
                Year = year;
                Always = always;
                Never = never;
                Monday = monday;
                Tuesday = tuesday;
                Wednesday = wednesday;
                Thursday = thursday;
                Friday = friday;
                Saturday = saturday;
                Sunday = sunday;
            }
        }

        private static readonly Dictionary<CultureInfo, DescriptorKeywords> LocalizedKeywords = new Dictionary<CultureInfo, DescriptorKeywords>
        {
            {CultureInfo.InvariantCulture, 
                new DescriptorKeywords("AND", "OR", "BUT", "INTERSECTING", "BETWEEN", "EVERY", "DAY", "MONTH", "WEEK", "YEAR", "ALWAYS", "NEVER",
                    "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY")},
            {new CultureInfo("fr"), 
                new DescriptorKeywords("ET", "OU", "SAUF", "RECOUPANT", "ENTRE", "CHAQUE", "JOUR", "MOIS", "SEMAINE", "ANNEE", "TOUJOURS", "JAMAIS",
                    "LUNDI", "MARDI", "MERCREDI", "JEUDI", "VENDREDI", "SAMEDI", "DIMANCHE")}
        };

        public static DescriptorKeywords Keywords => LocalizedKeywords[CultureInfo.InvariantCulture];

        public static string Translate(string str, CultureInfo current, CultureInfo wanted)
        {
            if (Equals(current, wanted)) return str;
            if (!LocalizedKeywords.ContainsKey(wanted)) wanted = CultureInfo.InvariantCulture;

            var currentKeywords = LocalizedKeywords[current];
            var wantedKeywords = LocalizedKeywords[wanted];

            str = Regex.Replace(str, @"\b" + currentKeywords.And + @"\b", wantedKeywords.And);
            str = Regex.Replace(str, @"\b" + currentKeywords.Or + @"\b", wantedKeywords.Or);
            str = Regex.Replace(str, @"\b" + currentKeywords.But + @"\b", wantedKeywords.But);
            str = Regex.Replace(str, @"\b" + currentKeywords.Intersecting + @"\b", wantedKeywords.Intersecting);
            str = Regex.Replace(str, @"\b" + currentKeywords.Between + @"\b", wantedKeywords.Between);
            str = Regex.Replace(str, @"\b" + currentKeywords.Every + @"\b", wantedKeywords.Every);
            str = Regex.Replace(str, @"\b" + currentKeywords.Day + @"\b", wantedKeywords.Day);
            str = Regex.Replace(str, @"\b" + currentKeywords.Month + @"\b", wantedKeywords.Month);
            str = Regex.Replace(str, @"\b" + currentKeywords.Week + @"\b", wantedKeywords.Week);
            str = Regex.Replace(str, @"\b" + currentKeywords.Year + @"\b", wantedKeywords.Year);
            str = Regex.Replace(str, @"\b" + currentKeywords.Always + @"\b", wantedKeywords.Always);
            str = Regex.Replace(str, @"\b" + currentKeywords.Never + @"\b", wantedKeywords.Never);
            str = Regex.Replace(str, @"\b" + currentKeywords.Monday + @"\b", wantedKeywords.Monday);
            str = Regex.Replace(str, @"\b" + currentKeywords.Tuesday + @"\b", wantedKeywords.Tuesday);
            str = Regex.Replace(str, @"\b" + currentKeywords.Wednesday + @"\b", wantedKeywords.Wednesday);
            str = Regex.Replace(str, @"\b" + currentKeywords.Thursday + @"\b", wantedKeywords.Thursday);
            str = Regex.Replace(str, @"\b" + currentKeywords.Friday + @"\b", wantedKeywords.Friday);
            str = Regex.Replace(str, @"\b" + currentKeywords.Saturday + @"\b", wantedKeywords.Saturday);
            str = Regex.Replace(str, @"\b" + currentKeywords.Sunday + @"\b", wantedKeywords.Sunday);

            return str;
        }
    }
}
