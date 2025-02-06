using Heos.Tools.XmlParser.Models;

namespace Heos.Tools.ClassGenerator.Generator;

public class FileGenerator
{
    private readonly ResponseClassGenerator _responseGen = new();
    private readonly RequestClassGenerator _requestGen = new();
    private readonly HeaderGenerator _headerGen = new();

    public async Task Generate(
        string dirPath,
        string rootNamespace,
        IEnumerable<Command> requests,
        SpecificationInfo info)
    {
        foreach (var request in requests)
        {
            await GenerateFile(dirPath, request, rootNamespace, info);
        }
    }

    private string GenerateFileContent(
        Command command,
        string rootNamespace,
        SpecificationInfo info)
    {
        var header = _headerGen
            .Get($"{rootNamespace}.{command.Group}", info, command.GetResponseFields().Count != 0);
        var requestContents = _requestGen.Get(command);
        var responseContents = _responseGen.Get(command);

        return header + requestContents + responseContents;
    }

    private async Task GenerateFile(
        string requestDirPath,
        Command command,
        string rootNamespace,
        SpecificationInfo info)
    {
        var content = GenerateFileContent(command, rootNamespace, info);

        var dirPath = Path.Combine(requestDirPath, command.Group);
        var path = Path.Combine(requestDirPath, command.Group, $"{command.Name}.g.cs");

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        await File.WriteAllTextAsync(path, content);
    }
}