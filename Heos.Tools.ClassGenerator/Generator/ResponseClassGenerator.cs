using System.Text;
using Heos.Common.Extensions;
using Heos.Tools.ClassGenerator.Constants;
using Heos.Tools.ClassGenerator.Enums;
using Heos.Tools.ClassGenerator.Extensions;
using Heos.Tools.XmlParser.Models;

namespace Heos.Tools.ClassGenerator.Generator;

internal class ResponseClassGenerator
{
    public string Get(Command command)
    {
        if (command.GetResponseFields().Count == 0)
            return "";

        return command.ResponseType.ToLowerInvariant() switch
        {
            "class" => HandleCase(command, PayloadType.Poco),
            "list" => HandleCase(command, PayloadType.List),
            _ => throw new NotSupportedException()
        };
    }

    private string HandleCase(Command command, PayloadType payloadType)
    {
        var builder = new StringBuilder();

        builder.Append(ClassConstants.ResponseClassTemplate
            .Replace("{0}", GetType().GetAssemblyName())
            .Replace("{1}", GetType().GetAssemblyVersion())
            .Replace("{2}", command.Name)
            .Replace("{3}", GetPayloadParameterAction(payloadType).Invoke(command.Name)));

        builder.Append(ClassConstants.ResponsePayloadClassTemplate
            .Replace("{0}", GetType().GetAssemblyName())
            .Replace("{1}", GetType().GetAssemblyVersion())
            .Replace("{2}", command.Name)
            .Replace("{3}", GetPayloadParameters(command.GetResponseFields())));

        return builder.ToString();
    }

    private Func<string, string> GetPayloadParameterAction(PayloadType type) =>
        type switch
        {
            PayloadType.Poco => GetPocoPayloadParameter,
            PayloadType.List => GetListPayloadParameter,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Payload type is not valid.")
        };

    private string GetPayloadParameters(IEnumerable<ResponseField> fields)
    {
        var builder = new StringBuilder();
        builder.Append('\n');

        foreach (var field in fields)
        {
            builder.Append(ClassConstants.ResponsePropertyTemplate
                .Replace("{0}", field.Name)
                .Replace("{1}", field.Name.FirstCharToUpper()));
        }

        return builder.ToString();
    }

    private string GetListPayloadParameter(string requestName) =>
        $"\n\t[JsonProperty(\"payload\")]\n\tpublic List<{requestName}ResponsePayload>? Payload {{ get; set; }}\n";

    private string GetPocoPayloadParameter(string requestName) =>
        $"\n\t[JsonProperty(\"payload\")]\n\tpublic {requestName}ResponsePayload? Payload {{ get; set; }}\n";
}