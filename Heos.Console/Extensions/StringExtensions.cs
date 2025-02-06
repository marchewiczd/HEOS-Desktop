namespace Heos.Console.Extensions;

public static class StringExtensions
{
    public static object[] ToObjectArray(this string[] value) =>
        value.OfType<object>().ToArray();
}