using System.Text.RegularExpressions;

namespace Heos.Console.Helpers;

public partial class RegexHelper
{
    [GeneratedRegex("^((25[0-5]|(2[0-4]|1\\d|[1-9]|)\\d)\\.?\\b){4}$")]
    public static partial Regex Ipv4();

    [GeneratedRegex("^-([0-9]{9})\\b")]
    public static partial Regex Pid();
}