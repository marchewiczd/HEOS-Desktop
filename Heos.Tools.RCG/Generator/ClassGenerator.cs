using System.Reflection;
using Heos.Tools.RCG.Constants;
using Heos.Tools.RCG.Extensions;
using Heos.Tools.RCG.Models;
using Microsoft.CodeAnalysis;

namespace Heos.Tools.RCG.Generator;

public class ClassGenerator
{

    public async Task GenerateFromRequests(string requestDirPath, string rootNamespace, IEnumerable<Request> requests)
    {
        var classDescriptions = TransformAll(rootNamespace, requests);

        foreach (var description in classDescriptions)
        {
            await GenerateClass(requestDirPath, description);
        }
    }

    private async Task GenerateClass(string requestDirPath, ClassDescription description)
    {
        var dirPath = Path.Combine(requestDirPath, description.Group);
        var path = Path.Combine(requestDirPath, description.Group, description.FileName);

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        await File.WriteAllTextAsync(path, description.Content);
    }

    private IEnumerable<ClassDescription> TransformAll(string rootNamespace, IEnumerable<Request> requests) =>
        requests.Select(request => TransformRequestToClassDescription(rootNamespace, request));

    private ClassDescription TransformRequestToClassDescription(string rootNamespace, Request request)
    {
        var description = new ClassDescription
        {
            Group = GetEndpointGroup(request.Endpoint),
            ClassName = TransformEndpointToName(request.Endpoint)
        };

        description.Content = ClassConstants.RequestClassTemplate
            .Replace("{0}", $"{rootNamespace}.{description.Group}")
            .Replace("{1}", description.ClassName)
            .Replace("{2}", GetConstructorParameters(request.GetParameters()))
            .Replace("{3}", request.Endpoint[1..])
            .Replace("{4}", GetInitParameters(request.GetParameters()))
            .Replace("{5}", GetType().Assembly.GetName().Name)
            .Replace("{6}", GetType().Assembly.GetName().Version?.ToString(3));

        return description;
    }


    private string TransformEndpointToName(string endpoint)
    {
        var command = endpoint.Substring(endpoint.LastIndexOf('/') + 1);
        var segments = command.Split('_').Select(x => x.FirstCharToUpper());

        return string.Concat(segments);
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

    private string GetEndpointGroup(string endpoint)
    {
        if (endpoint.Count(x => x == '/') == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(endpoint),
                "Endpoint is not specified correctly - missing / character");
        }

        if (endpoint.Count(x => x == '/') == 1)
        {
            return endpoint.Substring(1);
        }

        return endpoint.Substring(1, endpoint.LastIndexOf('/') - 1).FirstCharToUpper();
    }
}