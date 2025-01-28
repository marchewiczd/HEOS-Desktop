using System.Text.RegularExpressions;

namespace Heos.Tools.XmlParser.Constants;

public partial class RegexExpr
{
    [GeneratedRegex(@"(?i)\b(request)\b")]
    public static partial Regex RequestNodeRegex();

    [GeneratedRegex(@"(?i)\b(command|group)\b")]
    public static partial Regex CommandNodeRegex();

    [GeneratedRegex(@"(?i)\b(response)\b")]
    public static partial Regex ResponseNodeRegex();
}