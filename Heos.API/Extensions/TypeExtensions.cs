using System.Reflection;

namespace Heos.API.Extensions;

public static class TypeExtensions
{
    public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute(this Type type, Type attributeType) => 
        type.GetProperties().Where(prop => prop.IsDefined(attributeType, false));
}