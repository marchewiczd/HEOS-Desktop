using System.Text;

namespace Heos.Tools.RCG.Models;

public class Request
{
    private readonly List<RequestParameter> _parameters;

    public Request(string endpoint, string description = "")
    {
        Endpoint = endpoint;
        Description = description;
        _parameters = [];
    }

    public string Endpoint { get; private set; }

    public string Description { get; private set; }

    public void AddParameter(RequestParameter parameter) =>
        _parameters.Add(parameter);

    public List<RequestParameter> GetParameters() => _parameters;
}