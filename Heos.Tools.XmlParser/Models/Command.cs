using Heos.Common.Extensions;

namespace Heos.Tools.XmlParser.Models;

public class Command
{
    private readonly List<RequestParameter> _parameters;
    private readonly List<ResponseField> _responseFields;

    public Command(string endpoint, string description = "")
    {
        Endpoint = endpoint;
        Description = description;
        Group = GetEndpointGroup();
        Name = TransformEndpointToName();
        _parameters = [];
        _responseFields = [];
    }

    public string Endpoint { get; private set; }
    public string Description { get; private set; }
    public string Group { get; private set; }
    public string Name { get; private set; }
    public string ResponseType { get; set; } = string.Empty;

    public void AddParameter(RequestParameter parameter) =>
        _parameters.Add(parameter);

    public void AddResponseField(ResponseField field) =>
        _responseFields.Add(field);

    public List<RequestParameter> GetParameters() => _parameters;
    public List<ResponseField> GetResponseFields() => _responseFields;

    private string GetEndpointGroup()
    {
        if (Endpoint.All(x => x != '/'))
        {
            throw new ArgumentOutOfRangeException(nameof(Endpoint),
                "Endpoint is not specified correctly - missing / character");
        }

        if (Endpoint.Count(x => x == '/') == 1)
        {
            return Endpoint.Substring(1);
        }

        return Endpoint.Substring(1, Endpoint.LastIndexOf('/') - 1).FirstCharToUpper();
    }

    private string TransformEndpointToName()
    {
        var command = Endpoint.Substring(Endpoint.LastIndexOf('/') + 1);
        return command.SnakeCaseToPascalCase();
    }
}