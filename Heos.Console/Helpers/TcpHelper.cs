using Heos.API.Enums.HEOS;
using Heos.API.Extensions;
using Heos.API.Models.HEOS;
using Heos.API.Models.HEOS.Player;
using Heos.API.Services;
using Heos.Console.Configuration;
using Heos.Console.Extensions;
using static System.Console;

namespace Heos.Console.Helpers;

public class TcpHelper
{
    private readonly RequestMapper _requestMapper = new();

    public void SsdpDiscover()
    {
        var ssdp = new SsdpService(TimeSpan.FromSeconds(2));
        foreach (var entry in ssdp.Discover())
        {
            WriteLine(entry.GetLocationIpAddress());
        }
    }

    public void HandleHeosRequest(params string[] args)
    {
        var parameters = args.Length > 3 ? args[2..] : null;
        var request = _requestMapper.CreateRequestInstance(args);
        Config.TryInject(ref args, "host");

        var tcpService = new TcpService(args[1]);
        WriteLine(tcpService.Send(request.Build()));
    }

    public void Wake(params string[] args)
    {
        if (args.Length == 0)
        {
            WriteLine("IP was not provided!");
            return;
        }

        WriteLine("Connecting...");
        Config.TryInject(ref args, "host");

        var tcpService = new TcpService(args[1]);
        WriteLine($"Connected to {args[1]}!");

        var pid = GetPid(tcpService, args[1]);
        if (string.IsNullOrEmpty(pid))
        {
            return;
        }

        SendWakeRequest(tcpService, pid);
    }

    private string GetPid(TcpService tcpService, string host)
    {
        WriteLine("Searching for PID...");
        var getPlayersRequest = new GetPlayersRequest();
        var getPlayersResponse = tcpService.Send<GetPlayersRequest, GetPlayersResponse>(getPlayersRequest);

        var pid = getPlayersResponse.Payload?.FirstOrDefault(r => r.Ip == host)?.Pid;

        if (pid is null)
        {
            WriteLine($"Cannot find PID for {host}");
            return "";
        }

        WriteLine($"PID for {host} found: {pid}");
        return pid;
    }

    private void SendWakeRequest(TcpService tcpService, string pid)
    {
        WriteLine($"Waking speaker...");
        var setPlayStateRequest = new SetPlayStateRequest(pid, "play");
        var response = tcpService.Send<SetPlayStateRequest, HeosResponse>(setPlayStateRequest);
        WriteLine(response.Result == RequestResult.Success ? "Speaker is live!" : "Cannot wake speaker!");
    }
}