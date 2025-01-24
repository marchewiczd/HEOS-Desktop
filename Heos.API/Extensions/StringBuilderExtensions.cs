using System.Text;

namespace Heos.API.Extensions;

public static class StringBuilderExtensions
{
    public static StringBuilder RemoveLast(this StringBuilder stringBuilder)
    {
        return stringBuilder.Remove(stringBuilder.Length - 1, 1);
    }
}