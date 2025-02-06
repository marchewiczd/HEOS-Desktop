using Heos.Tools.ClassGenerator.Constants;
using Heos.Tools.ClassGenerator.Extensions;
using Heos.Tools.XmlParser.Models;

namespace Heos.Tools.ClassGenerator.Generator;

internal class RequestClassGenerator
{
    public string Get(Command command)
    {
        return ClassConstants.RequestClassTemplate
            .Replace("{0}", GetType().GetAssemblyName())
            .Replace("{1}", GetType().GetAssemblyVersion())
            .Replace("{2}", command.Name)
            .Replace("{3}", GetConstructorParameters(command.GetParameters()))
            .Replace("{4}", command.Endpoint[1..])
            .Replace("{5}", GetInitParameters(command.GetParameters()));
    }

    private string GetConstructorParameters(List<RequestParameter> parameters)
    {
        var result = string.Empty;

        if (parameters.Count == 0)
            return result;

        for (var i = 0; i < parameters.Count; i++)
        {
            if (i == parameters.Count - 1)
            {
                result += $"string {parameters[i].Name}";
                break;
            }

            result += $"string {parameters[i].Name}, ";
        }

        return result;
    }

    private string GetInitParameters(List<RequestParameter> parameters)
    {
        var result = "\n\t\t{\n";

        if (parameters.Count == 0)
            return string.Empty;

        for (var i = 0; i < parameters.Count; i++)
        {
            if (i == parameters.Count - 1)
            {
                result += $"\t\t\t[\"{parameters[i].Name}\"] = {parameters[i].Name}\n\t\t}}";
                break;
            }

            result += $"\t\t\t[\"{parameters[i].Name}\"] = {parameters[i].Name},\n";
        }

        return result;
    }
}