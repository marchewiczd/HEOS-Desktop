namespace Heos.Tools.XmlParser.Models;

public class Request
{
    private readonly List<RequestParameter> _parameters;
    private readonly List<ResponseField> _responseFields;

    public Request(string endpoint, string description = "")
    {
        Endpoint = endpoint;
        Description = description;
        _parameters = [];
        _responseFields = [];
    }

    public string Endpoint { get; private set; }

    public string Description { get; private set; }
    public string ResponseType { get; set; } = string.Empty;

    public void AddParameter(RequestParameter parameter) =>
        _parameters.Add(parameter);

    public void AddResponseField(ResponseField field) =>
        _responseFields.Add(field);

    public List<RequestParameter> GetParameters() => _parameters;
    public List<ResponseField> GetResponseFields() => _responseFields;
}