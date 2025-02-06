using Heos.Console.Extensions;
using Heos.Console.Helpers;
using Heos.Console.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Heos.Console.Configuration;

public static class Config
{
    private const string FileName = "config";
    private static IConfiguration? _configuration;

    public static void Load(CliArguments cliArguments)
    {
        var builder = new ConfigurationBuilder().AddJsonFile(GetFilePath(), false, true);
        _configuration = builder.Build();
        cliArguments.Map(_configuration);
    }

    public static string? Get(string s)
    {
        return _configuration?[s];
    }

    public static void Set(string s, string value)
    {
        if (_configuration is null)
        {
            return;
        }

        _configuration[s] = value;
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
        var configDictionary = _configuration?.AsEnumerable().ToDictionary();
        var json = JsonConvert.SerializeObject(configDictionary);
        File.WriteAllText(GetFilePath(), json);
    }

    private static string GetFilePath()
    {
        var executableDirectory = Path.GetDirectoryName(Environment.ProcessPath);
        ArgumentNullException.ThrowIfNull(executableDirectory);

        return Path.Combine(executableDirectory, FileName);
    }
}