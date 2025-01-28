namespace Heos.Tools.XmlParser.Models;

public class Response
{
    public List<ResponseField> Fields { get; set; } = new();
    public string Type { get; set; } = String.Empty;
}