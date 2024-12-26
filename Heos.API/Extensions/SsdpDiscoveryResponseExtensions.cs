using Heos.API.Models.SSDP;

namespace Heos.API.Extensions;

public static class SsdpDiscoveryResponseExtensions
{
    public static string GetLocationIpAddress(this SsdpDiscoveryResponse response)
    {
        var startIndexOfIp = response.Location.IndexOf('/') + 2;
        var endIndexOfIp = response.Location.LastIndexOf(':');
        var substringLength = endIndexOfIp - startIndexOfIp;

        return response.Location.Substring(startIndexOfIp, substringLength);
    }
}