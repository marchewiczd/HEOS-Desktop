using Heos.API.JsonConverters;
using Newtonsoft.Json;

namespace Heos.API.Models.HEOS.Player;

public class GetPlayersRequest : HeosRequest
{
    public GetPlayersRequest()
    {
        Command = "player/get_players";
        Parameters = new();
    }
}

[JsonConverter(typeof(JsonPathConverter))]
public class GetPlayersResponse : HeosResponse
{
    [JsonProperty("payload")]
    public List<GetPlayersResponsePayload>? Payload { get; set; }
}

[JsonConverter(typeof(JsonPathConverter))]
public record GetPlayersResponsePayload
{
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("pid")] public string? Pid { get; set; }
    [JsonProperty("gid")] public string? Gid { get; set; }
    [JsonProperty("model")] public string? Model { get; set; }
    [JsonProperty("version")] public string? Version { get; set; }
    [JsonProperty("network")] public string? Network { get; set; }
    [JsonProperty("lineout")] public string? LineOut { get; set; }
    [JsonProperty("control")] public string? Control { get; set; }
    [JsonProperty("serial")] public string? Serial { get; set; }
}