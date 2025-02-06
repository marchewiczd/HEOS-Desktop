namespace Heos.Console.Extensions;

public static class StringExtensions
{
    public static object[] ToObjectArray(this string[] value) =>
        value.OfType<object>().ToArray();

    public static string[] Inject(this string[] array, string value, int index) =>
        array[..index]
            .Concat([value])
            .Concat(array[index..])
            .ToArray();
}