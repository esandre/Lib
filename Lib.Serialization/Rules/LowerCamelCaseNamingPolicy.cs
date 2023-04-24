using System.Text;
using System.Text.Json;

namespace ConsoleApi.Serialization.Rules;

public class LowerCamelCaseNamingPolicy : JsonNamingPolicy
{
    private static readonly JsonNamingPolicy RegularCamelCase = CamelCase;

    public override string ConvertName(string name)
    {
        var regularCamelCase = RegularCamelCase.ConvertName(name);
        var builder = new StringBuilder(regularCamelCase);

        var firstLetter = regularCamelCase[..1].ToLowerInvariant();
        builder[0] = firstLetter[0];

        return builder.ToString();
    }
}