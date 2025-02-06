using Heos.Console.Configuration;
using Heos.Console.Handlers;
using Heos.Console.Helpers;
using Heos.Console.Model;

namespace Heos.Console;

public class Program
{
    private static void Main(string[] args)
    {
        var cliArguments = new CliArguments();
        cliArguments.Map(args);

        Config.Load(cliArguments);
        new CliHandler().Handle(cliArguments);
    }
}