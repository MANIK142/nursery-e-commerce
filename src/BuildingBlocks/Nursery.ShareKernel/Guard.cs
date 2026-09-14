
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Nursery.ShareKernel;

public static class Guard
{
    public static string AgainstNullOrWhiteSpace(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"'{paramName}' cannot be null or whitespace.", paramName);
        return value;
    }

    public static decimal AgainstNegativeOrZero(decimal value, string paramName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(paramName, value, $"'{paramName}' must be greater than zero.");
        return value;
    }

    public static int AgainstNegative(int value, string paramName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(paramName, value, $"'{paramName}' cannot be negative.");
        return value;
    }
    public static decimal AgainstNegative(decimal value, string paramName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(paramName, value, $"'{paramName}' cannot be negative.");
        return value;
    }

    public static T AgainstDefault<T>([NotNull] T input,string paramName)
    {
        if (EqualityComparer<T>.Default.Equals(input, default))
        {
            throw new ArgumentException(
                $"Required input {paramName} cannot be default.",
                paramName);
        }
        return input;
    }

}
