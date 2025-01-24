using System.Text;
using Heos.API.Extensions;

namespace Heos.API.Models.HEOS;

public abstract class HeosRequest
{
    protected string Command { get; init; } = string.Empty;
    protected Dictionary<string, string>? Parameters { get; init; } = null;

    private string GetQueryParams()
    {
        var stringBuilder = new StringBuilder("?");

        if (Parameters is null || Parameters.Count == 0)
        {
            return string.Empty;
        }

        foreach (var param in Parameters)
        {
            stringBuilder.Append($"{param.Key}={param.Value}&");
        }

        stringBuilder.RemoveLast();

        return stringBuilder.ToString();
    }

    public string Build() => Command + GetQueryParams();
}