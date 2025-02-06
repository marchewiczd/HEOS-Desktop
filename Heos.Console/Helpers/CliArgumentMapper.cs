using Heos.Console.Model;
using Microsoft.Extensions.Configuration;

namespace Heos.Console.Helpers;

public static class CliArgumentMapper
{
    public static void Map(this CliArguments cliArguments, string[] args)
    {
        if (args.Length == 0)
            return;

        cliArguments.Command = args[0];

        for(var i = 1; i < args.Length; i++)
        {
            if (RegexHelper.Ipv4().Match(args[i]).Success)
            {
                cliArguments.Host = args[i];
                continue;
            }

            if (RegexHelper.Pid().Match(args[i]).Success)
            {
                cliArguments.Pid = args[i];
                continue;
            }

            cliArguments.Parameters.Add(args[i]);
        }
    }

    public static void Map(this CliArguments cliArguments, IConfiguration configuration)
    {
        var arguments = configuration.Get<CliArguments>();
        if (arguments is null)
            return;

        cliArguments.Pid = string.IsNullOrEmpty(arguments.Pid) ? cliArguments.Pid : arguments.Pid;
        cliArguments.Host = string.IsNullOrEmpty(arguments.Host) ? cliArguments.Host : arguments.Host;
    }
}