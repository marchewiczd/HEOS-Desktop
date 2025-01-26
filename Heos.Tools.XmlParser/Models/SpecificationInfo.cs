namespace Heos.Tools.XmlParser.Models;

public record SpecificationInfo
{
    public Version? Version { get; init; }
    public Uri? Uri { get; init; }
}