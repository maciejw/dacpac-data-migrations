using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DacpacDataMigrations;

/// <summary>
/// Compares two strings that contain numbers in their file names.
/// </summary>
internal class NumbersInFileNameComparer : IComparer<string>
{
    /// <summary>
    /// Compares two strings that contain numbers in their file names.
    /// </summary>
    /// <param name="x">The first string to compare.</param>
    /// <param name="y">The second string to compare.</param>
    /// <returns>An integer that indicates the relative order of the strings being compared.</returns>
    public int Compare(string? x, string? y)
    {
        var result = 0;

        var xMatches = x.GetMatches();
        var yMatches = y.GetMatches();

        var maxMatchesLength = Math.Max(xMatches.Length, yMatches.Length);

        for (var i = 0; i < maxMatchesLength; i++)
        {
            var xMatch = xMatches.GetValueOrDefault(i);
            var yMatch = yMatches.GetValueOrDefault(i);

            var xNumberSegments = xMatch.GetNumberSegments();
            var yNumberSegments = yMatch.GetNumberSegments();

            if (xNumberSegments.Any() || yNumberSegments.Any())
            {
                var maxSegmentsLength = Math.Max(xNumberSegments.Length, yNumberSegments.Length);

                for (var j = 0; j < maxSegmentsLength; j++)
                {
                    var xNumberSegmentsValue = xNumberSegments.GetValueOrDefault(j, "0");
                    var yNumberSegmentsValue = yNumberSegments.GetValueOrDefault(j, "0");

                    var xParseResult = int.TryParse(xNumberSegmentsValue, out var xNumber);
                    var yParseResult = int.TryParse(yNumberSegmentsValue, out var yNumber);

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
                var xRestValue = xMatch.GetNotNumbersValue();
                var yRestValue = yMatch.GetNotNumbersValue();
                result = DefaultStringComparer.StringComparer.Compare(xRestValue, yRestValue);
                if (result == 0) continue;
                if (result < 0) return -1;
                if (result > 0) return 1;
            }
        }
        return result;
    }
}
