using System.Text.Json.Serialization;

namespace Heos.Console.Model;

public class CliArguments
{
    public string Command { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string Pid { get; set; } = string.Empty;
    public List<string> Parameters { get; set; } = new();
}