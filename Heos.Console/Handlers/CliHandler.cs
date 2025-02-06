using Heos.Common.Extensions;
using Heos.Console.Helpers;
using static System.Console;

namespace Heos.Console.Handlers;

public class CliHandler
{
    private readonly TcpHelper _tcpHelper = new();
    private readonly RequestMapper _requestMapper = new();

    public void Handle(params string[] args)
    {
        if (args.Length == 0)
        {
            Help();
            return;
        }

        if (args[0].StartsWith('-'))
        {
            HandleOptions(args[0]);
            return;
        }

        HandleCommand(args);
    }

    private void HandleOptions(string option)
    {
        switch (option)
        {
            case "--version" or "-v":
                PrintVersion();
                break;

            case "--help" or "-h":
                Help();
                break;
        }
    }

    private void HandleCommand(params string[] args)
    {
        switch (args[0].ToLowerInvariant())
        {
            case "discover":
                _tcpHelper.SsdpDiscover();
                break;

            case "wake":
                _tcpHelper.Wake(args);
                break;

            default:
                _tcpHelper.HandleHeosRequest(args);
                break;
        }
    }

    private void Help()
    {
        PrintHelpIntro();
        PrintHeosCommands();
    }

    private void PrintHelpIntro()
    {
        WriteLine("""
                  Usage:
                    heos [command] [ip] [request parameters]
                    heos [options]
                  
                  Available options:
                    --version -v
                    --help -h
                    
                  Available commands:
                    Discover
                    Wake
                  """);
    }

    private void PrintHeosCommands()
    {
        foreach (var requestType in _requestMapper.GetAll())
        {
            WriteLine($"  {requestType.Name.Replace("Request", "")}");
        }
    }

    private void PrintVersion()
    {
        WriteLine(GetType().GetAssemblyVersion());
    }
}