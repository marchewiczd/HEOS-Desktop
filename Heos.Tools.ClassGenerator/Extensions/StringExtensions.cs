namespace Heos.Tools.ClassGenerator.Extensions;

public static class StringExtensions
{
    public static string FirstCharToUpper(this string value)
    {
        return char.ToUpper(value.First()) + value[1..];
    }
}