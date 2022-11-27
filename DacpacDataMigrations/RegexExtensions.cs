
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DacpacDataMigrations;

internal static class RegexExtensions
{
    public static readonly Regex NumbersRegex = new(@"(?<numbers>(?:\d\.*)+)", RegexOptions.Compiled);
    public static readonly Regex NotNumbersRegex = new(@"(?<notNumbers>[^\d]+((?<![\d\.])))", RegexOptions.Compiled);

    public static Match[] GetMatches(this string? value)
    {
        var numberMatches = NumbersRegex.Matches($"{value}").Cast<Match>();
        var nonNumberMatches = NotNumbersRegex.Matches($"{value}").Cast<Match>();

        return numberMatches.Union(nonNumberMatches).OrderBy(m => m.Index).ToArray();
    }

    public static T? GetValueOrDefault<T>(this T[] array, int index, T? @default = default)
    {
        if (index < array.Length)
        {
            return array[index];
        }
        return @default;
    }
    public static string GetNotNumbersValue(this Match? match)
    {
        return match.GetGroupValue("notNumbers");
    }
    public static string[] GetNumberSegments(this Match? match)
    {
        var value = match.GetGroupValue("numbers");
        if (string.IsNullOrEmpty(value))
        {
            return Array.Empty<string>();
        }
        return value.Split('.');
    }
    public static string GetGroupValue(this Match? match, string name)
    {
        var value = "";
        if (match?.Success == true)
        {
            var group = match.Groups[name];
            if (group?.Success == true)
            {
                value = group.Value;
            }
        }
        return value;
    }
}
