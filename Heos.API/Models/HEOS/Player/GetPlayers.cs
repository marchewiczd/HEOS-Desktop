namespace Heos.API.Models.HEOS.Player;

public class GetPlayersRequest : HeosRequest
{
    public GetPlayersRequest()
    {
        Command = "player/get_players";
    }
}

public class GetPlayersResponse : HeosResponse
{
    public List<GetPlayersResponsePayload>? Payload { get; set; }
}

public record GetPlayersResponsePayload
{
    public string? Name { get; set; }
    public string? Pid { get; set; }
    public string? Gid { get; set; }
    public string? Model { get; set; }
    public string? Version { get; set; }
    public string? Network { get; set; }
    public string? LineOut { get; set; }
    public string? Control { get; set; }
    public string? Serial { get; set; }
}