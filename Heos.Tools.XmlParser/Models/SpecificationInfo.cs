namespace Heos.Tools.XmlParser.Models;

public record SpecificationInfo
{
    public Version? Version { get; init; }
    public Uri? Url { get; init; }
}