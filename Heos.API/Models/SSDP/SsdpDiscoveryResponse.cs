using Heos.API.Attributes;
using Heos.API.Extensions;

namespace Heos.API.Models.SSDP;

public class SsdpDiscoveryResponse
{
    public string Status { get; set; } = string.Empty;
    
    [ResponseField("CACHE-CONTROL")]
    public string CacheControl { get; set; } = string.Empty;
    
    [ResponseField("EXT")]
    public string Ext { get; set; } = string.Empty;
    
    [ResponseField("LOCATION")]
    public string Location { get; set; } = string.Empty;
    
    [ResponseField("VERSIONS.UPNP.HEOS.COM")]
    public string Versions { get; set; } = string.Empty;
    
    [ResponseField("NETWORKID.UPNP.HEOS.COM")]
    public string NetworkId { get; set; } = string.Empty;
    
    [ResponseField("BOOTID.UPNP.ORG")]
    public string BootId { get; set; } = string.Empty;
    
    [ResponseField("IPCACHE.URL.UPNP.HEOS.COM")]
    public string IpCache { get; set; } = string.Empty;
    
    [ResponseField("SERVER")]
    public string Server { get; set; } = string.Empty;
    
    [ResponseField("ST")]
    public string SearchTarget { get; set; } = string.Empty;
    
    [ResponseField("USN")]
    public string Usn { get; set; } = string.Empty;
}