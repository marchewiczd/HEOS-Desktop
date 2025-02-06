using Heos.Console.Handlers;

namespace Heos.Console;

public class Program
{
    private static void Main(string[] args)
    {
        new CliHandler().Handle(args);
    }
}