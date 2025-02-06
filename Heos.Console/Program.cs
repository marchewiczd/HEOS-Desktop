using Heos.Console.Configuration;
using Heos.Console.Handlers;

namespace Heos.Console;

public class Program
{
    private static void Main(string[] args)
    {
        Config.Load();
        new CliHandler().Handle(args);
    }
}