using System.Text.RegularExpressions;

namespace Heos.Tools.RCG.Constants;

public partial class RegexExpr
{
    [GeneratedRegex(@"(?i)\b(parameter|argument)\b")]
    public static partial Regex ParameterNodeRegex();

    [GeneratedRegex(@"(?i)\b(command|group)\b")]
    public static partial Regex CommandNodeRegex();
}