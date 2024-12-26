using System.Text;
using Heos.API.Models.SSDP;

namespace Heos.API.Extensions;

public static class SsdpDiscoveryRequestExtensions
{
    /// <summary>
    /// Gets bytes from string message in UTF8 encoding.
    /// </summary>
    public static byte[] ToUtf8Bytes(this SsdpDiscoveryRequest request) =>
        Encoding.UTF8.GetBytes(request.ToString());
}