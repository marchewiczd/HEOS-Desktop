using System.Text;

namespace Heos.Tools.RCG.Models;

public class Request
{
    private readonly List<RequestParameter> _parameters;

    public Request(string endpoint, string description = "")
    {
        Endpoint = endpoint;
        Description = description;
        _parameters = new();
    }

    public string Endpoint { get; private set; }

    public string Description { get; private set; }

    public string GetRequestString() =>
        $"heos://{Endpoint}{GetQueryString()}";

    public void UpdateDescription(string description) =>
        Description = description;

    public void AppendEpFragment(string fragment) =>
        Endpoint += $"/{fragment}";

    public void AddParameter(RequestParameter parameter) =>
        _parameters.Add(parameter);

    public List<RequestParameter> GetParameters() => _parameters;

    private string GetQueryString()
    {
        if (_parameters is null || _parameters.Count == 0)
            return "";

        var sb = new StringBuilder();
        sb.Append('?');

        _parameters.ForEach(x => sb.Append($"{x.Name.ToLowerInvariant()}={x.Value.ToLowerInvariant()}"));

        return sb.ToString();
    }
}