namespace Heos.API.Models.HEOS;

public abstract class HeosRequest
{
    public string Command { get; protected init; }
}