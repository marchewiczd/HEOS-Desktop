using System.Xml;

namespace Heos.Tools.XmlParser.Extensions;

public static class XmlExtensions
{
    public static string? GetAttributeValue(
        this XmlElement element, 
        string attributeName) => 
        element.Attributes[attributeName]?.Value;
    
    public static string? GetAttributeValue(
        this XmlNode node, 
        string attributeName) => 
        node.Attributes?[attributeName]?.Value;
}