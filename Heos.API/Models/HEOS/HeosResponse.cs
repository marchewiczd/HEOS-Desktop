using Heos.API.Enums.HEOS;
using Heos.API.JsonConverters;
using Newtonsoft.Json;

namespace Heos.API.Models.HEOS;

[JsonConverter(typeof(JsonPathConverter))]
public class HeosResponse
{
    [JsonProperty("heos.command")]
    public required string Command { get; set; }
    
    [JsonProperty("heos.result")]
    public required RequestResult Result { get; set; }
    
    [JsonProperty("heos.message")]
    public required string Message { get; set; }
}