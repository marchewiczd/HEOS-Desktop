using System.Text.RegularExpressions;
using Heos.Console.Extensions;
using Heos.Console.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Heos.Console.Configuration;

public static class Config
{
    private const string FileName = "config";
    private static IConfiguration? Configuration;

    public static void Load()
    {
        var builder = new ConfigurationBuilder().AddJsonFile(FileName, false, true);
        Configuration = builder.Build();
    }

    public static string? Get(string s)
    {
        return Configuration?[s];
    }

    public static void Set(string s, string value)
    {
        if (Configuration is null)
        {
            return;
        }

        Configuration[s] = value;
        Save();
    }

    public static bool TryInject(ref string[] args, string key, int index = 1)
    {
        var value = "";

        if (key.Equals("host"))
        {
            if (args.Any(x => RegexHelper.Ipv4().Match(x).Success))
            {
                return false;
            }

            value = Get(key);

            if (value is null)
            {
                return false;
            }
        }

        if (key.Equals("pid"))
        {
            if (args.Any(x => RegexHelper.Pid().Match(x).Success))
            {
                return false;
            }

            value = Get(key);

            if (value is null)
            {
                return false;
            }
        }

        args = args.Inject(value, index);

        return true;
    }

    private static void Save()
    {
        var configDictionary = Configuration?.AsEnumerable().ToDictionary();
        var json = JsonConvert.SerializeObject(configDictionary);
        File.WriteAllText(FileName, json);
    }
}