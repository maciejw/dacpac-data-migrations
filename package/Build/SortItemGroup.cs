using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class SortItemGroup : ITask
{
    public IBuildEngine BuildEngine { get; set; }
    public ITaskHost HostObject { get; set; }
    public ITaskItem[] In { get; set; }
    public ITaskItem[] Out { get; set; }
    public bool Execute()
    {
        Out = In.OrderBy(i => i.ItemSpec, new NumbersInFileNameComparer()).ToArray();
        return true;
    }
}

public class NumbersInFileNameComparer : IComparer<string>
{
    readonly Regex numbers = new Regex(@"(?<numbers>(?:\.?\d+)+)", RegexOptions.Compiled);
    readonly Regex rest = new Regex(@"(?<rest>(?:[^\|])+)", RegexOptions.Compiled);

    readonly StringComparer stringComparer = StringComparer.CurrentCultureIgnoreCase;
    public int Compare(string x, string y)
    {
        int result = 0;

        Match[] xMatches = GetMatches(x);
        Match[] yMatches = GetMatches(y);

        int maxMatchesLength = Math.Max(xMatches.Length, yMatches.Length);

        for (int i = 0; i < maxMatchesLength; i++)
        {
            Match xMatch = xMatches.SafeGetItem(i);
            Match yMatch = yMatches.SafeGetItem(i);

            string xRestValue = xMatch.GetRestValue();
            string yRestValue = yMatch.GetRestValue();

            string[] xNumberSegments = xMatch.GetNumberSegments();
            string[] yNumberSegments = yMatch.GetNumberSegments();

            if (xNumberSegments.Any() || yNumberSegments.Any())
            {
                var maxSegmentsLength = Math.Max(xNumberSegments.Length, yNumberSegments.Length);

                for (int j = 0; j < maxSegmentsLength; j++)
                {
                    string xNumberSegmentsValue = xNumberSegments.SafeGetItem(j, "0");
                    string yNumberSegmentsValue = yNumberSegments.SafeGetItem(j, "0");

                    int xNumber = 0; int yNumber = 0;
                    bool xParseResult = int.TryParse(xNumberSegmentsValue, out xNumber);
                    bool yParseResult = int.TryParse(yNumberSegmentsValue, out yNumber);

                    if (xParseResult || yParseResult)
                    {
                        result = xNumber.CompareTo(yNumber);
                        if (result == 0) continue;
                        return result;
                    }
                }
            }
            else
            {
                result = this.stringComparer.Compare(xRestValue, yRestValue);
                if (result == 0) continue;
                return result;
            }
        }
        return result;
    }
    Match[] GetMatches(string value)
    {
        var valueWithoutNumbers = numbers.Replace(value, m => new string('|', m.Length));

        var numberMatches = numbers.Matches(value).Cast<Match>();
        var restMatches = rest.Matches(valueWithoutNumbers).Cast<Match>();

        return numberMatches.Union(restMatches).OrderBy(m => m.Index).ToArray();
    }
}
public static class Extensions
{
    public static T SafeGetItem<T>(this T[] arr, int index, T @default = default(T))
    {
        if (index < arr.Length) return arr[index];
        return @default;
    }
    public static string GetRestValue(this Match match)
    {
        return match.GetGroupValue("rest");
    }
    public static string[] GetNumberSegments(this Match match)
    {
        var value = match.GetGroupValue("numbers");
        if (string.IsNullOrEmpty(value)) return new string[0];
        return value.Split('.');
    }
    public static string GetGroupValue(this Match match, string name)
    {
        string value = "";
        if (match != null && match.Success)
        {
            var group = match.Groups[name];
            if (group.Success) value = group.Value;
        }
        return value;
    }
}
