using System;

namespace DacpacDataMigrations;

internal class DefaultStringComparer
{
    internal static readonly StringComparer StringComparer = StringComparer.OrdinalIgnoreCase;
}
