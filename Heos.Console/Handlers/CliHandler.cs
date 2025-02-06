using Heos.Common.Extensions;
using Heos.Console.Configuration;
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

        if (args[0].Equals("config"))
        {
            HandleConfig(args[1..]);
            return;
        }

        HandleCommand(args);
    }

    #region Options

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



    #endregion

    #region Help

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
                    heos config
                    
                  Available options:
                    --version -v
                    --help -h
                    
                  Config:
                    get [config]
                    set [config] [value]
                    
                  Available commands:
                    Discover
                    Wake
                  """);
    }

    private void PrintVersion()
    {
        WriteLine(GetType().GetAssemblyVersion());
    }

    #endregion

    #region HEOS commands

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

    private void PrintHeosCommands()
    {
        foreach (var requestType in _requestMapper.GetAll())
        {
            WriteLine($"  {requestType.Name.Replace("Request", "")}");
        }
    }

    #endregion

    #region Config

    private void HandleConfig(params string[] args)
    {
        switch (args[0])
        {
            case "set":
                Config.Set(args[1], args[2]);
                WriteLine($"{args[1]} was set to {args[2]}");
                break;

            case "get":
                var value = Config.Get(args[1]);
                if (value is null)
                {
                    WriteLine($"Config for \"{args[1]}\" not found.");
                    return;
                }

                WriteLine(value);
                break;

            default:
                WriteLine($"{args[0]} was not recognized.");
                break;
        }
    }

    #endregion
}