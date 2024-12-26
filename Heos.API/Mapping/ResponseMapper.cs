using Heos.API.Attributes;
using Heos.API.Extensions;
using Heos.API.Models.SSDP;

namespace Heos.API.Mapping;

public static class ResponseMapper
{
    public static SsdpDiscoveryResponse MapSsdpDiscovery(string responseString)
    {
        var responseStringArray = responseString.Split("\r\n");
        var response = new SsdpDiscoveryResponse();
        
        if (responseStringArray.Length == 0)
            return response;
        
        response.Status = responseStringArray[0];
        
        var props = response.GetType().GetPropertiesWithAttribute(typeof(ResponseFieldAttribute));

        foreach (var prop in props)
        {
            var description = 
                ((ResponseFieldAttribute)prop.GetCustomAttributes(typeof(ResponseFieldAttribute), false).First())
                .Description;
            var responseValue = responseStringArray.FirstOrDefault(x => x.Contains(description));

            if (responseValue is null || responseValue.Length <= description.Length + 2)
                continue;

            responseValue = responseValue.Substring(description.Length + 2);
            prop.SetValue(response, responseValue);
        }

        return response;
    }
}