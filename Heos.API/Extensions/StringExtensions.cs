namespace Heos.API.Extensions;

public static class StringExtensions
{
    public static int SecondToLastIndex(this string str, char c)
    {
        if (str.Count(character => character == c) < 2)
            return 0;

        var lastIndexOfC = str.LastIndexOf(c);
        
        if (lastIndexOfC - 1 < 0)
            return 0;
        
        return str.Substring(0, lastIndexOfC - 1).LastIndexOf(c);
    }
}