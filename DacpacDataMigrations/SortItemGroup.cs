using Microsoft.Build.Framework;

using System;
using System.Collections.Generic;
using System.Linq;

namespace DacpacDataMigrations;

/// <summary>
/// Represents a task that sorts an array of ITaskItem objects based on their ItemSpec property.
/// </summary>
public class SortItemGroup : ITask
{
    /// <summary>
    /// Gets or sets the build engine interface.
    /// </summary>
    public IBuildEngine? BuildEngine { get; set; }

    /// <summary>
    /// Gets or sets the task host object.
    /// </summary>
    public ITaskHost? HostObject { get; set; }

    /// <summary>
    /// Gets or sets the input array of ITaskItem objects to be sorted.
    /// </summary>
    [Required]
    public ITaskItem[]? In { get; set; }

    /// <summary>
    /// Gets or sets the output array of sorted ITaskItem objects.
    /// </summary>
    [Output]
    public ITaskItem[]? Out { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to compare numbers in the ItemSpec property of the ITaskItem objects as numbers or as strings.
    /// </summary>
    public bool CompareNumbersInItemsAsNumbers { get; set; } = true;

    /// <summary>
    /// Sorts the input array of ITaskItem objects based on their ItemSpec property and returns true.
    /// </summary>
    /// <returns>True if the task was successful; otherwise, false.</returns>
    public bool Execute()
    {
        Out = In?.OrderBy(i => i.ItemSpec, GetComparer(CompareNumbersInItemsAsNumbers)).ToArray();
        return true;
    }

    private static readonly NumbersInFileNameComparer _numbersInFileNameComparer = new();

    internal static IComparer<string> GetComparer(bool compareNumbersInFileNamesAsNumbers)
    {
        if (compareNumbersInFileNamesAsNumbers)
        {
            return _numbersInFileNameComparer;
        }
        else
        {
            return DefaultStringComparer.StringComparer;
        }
    }
}
