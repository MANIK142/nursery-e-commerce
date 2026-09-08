
namespace Nursery.Catalog.Domain.Extenstions;

public static class StringExtentions
{
    public static T ToEnum<T>(this string value, bool ignoreCase = true, T defaultValue = default) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }

        return Enum.TryParse<T>(value, ignoreCase, out T result) ? result : defaultValue;
    }
}
