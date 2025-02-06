namespace Heos.Common.Extensions;

public static class StringExtensions
{
    public static string FirstCharToUpper(this string value) =>
        char.ToUpper(value.First()) + value[1..];

    public static string SnakeCaseToPascalCase(this string value) =>
        string.Concat(value.Split('_').Select(x => x.FirstCharToUpper()));
}