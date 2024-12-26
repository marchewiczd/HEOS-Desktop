namespace Heos.API.Models.SSDP;

public class SsdpDiscoveryRequest
{
    public string Method => "M-SEARCH * HTTP/1.1";
    public string Man => "ssdp:discover"; 
    public required string Host { get; init; }
    public required string SearchTarget { get; init; }
    public required string MaxWaitResponseTime { get; init; }

    public override string ToString() =>
        $"{Method}\r\nHOST: {Host}\r\nMAN: \"{Man}\"\r\nST: {SearchTarget}\r\nMX: {MaxWaitResponseTime}\r\n\r\n";
}