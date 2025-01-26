namespace Heos.Tools.ClassGenerator.Generator;

public class ClassDescription
{
    public string ClassName { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string FileName => $"{ClassName}.g.cs";
}