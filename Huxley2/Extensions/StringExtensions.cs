// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using System;

namespace Huxley2.Extensions
{
    public static class StringExtensions
    {
        public static bool NotNullAndEquals(this string? str, string matchString)
        {
            return str != null && str.Equals(matchString, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string RemoveString(this string? str, string removeString)
        {
            return str?.Replace(removeString, string.Empty, StringComparison.OrdinalIgnoreCase) ?? string.Empty;
        }
    }
}
