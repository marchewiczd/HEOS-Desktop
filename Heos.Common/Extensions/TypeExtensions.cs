namespace Heos.Common.Extensions;

public static class TypeExtensions
{
    public static string GetAssemblyName(this Type type) =>
        type.Assembly.GetName().Name ?? "";

    public static string GetAssemblyVersion(this Type type, int fieldCount = 3) =>
        type.Assembly.GetName().Version?.ToString(fieldCount) ?? "";
}