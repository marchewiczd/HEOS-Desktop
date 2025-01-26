using System.Data;
using System.Xml;
using Heos.Tools.XmlParser.Constants;
using Heos.Tools.XmlParser.Extensions;
using Heos.Tools.XmlParser.Models;

namespace Heos.Tools.XmlParser;

public class XmlParser
{
    private readonly List<Request> _requests = [];
    private readonly int _maxDepth;
    private readonly string _filePath;
    private XmlDocument _document;

    public XmlParser(string filePath, int maxDepth)
    {
        _filePath = filePath;
        _maxDepth = maxDepth;
    }

    public XmlParser() : this("RequestSpecification.xml", 5) { }

    public IEnumerable<Request> Parse()
    {
        _requests.Clear();
        _document = LoadXml(_filePath);
        GetSpecificationInfo();
        var childrenOfRoot = GetRootChildrenNodes();

        foreach (XmlNode node in childrenOfRoot)
        {
            RunParser(node);
        }

        return _requests;
    }

    public SpecificationInfo GetSpecificationInfo()
    {
        var rootNode = _document.DocumentElement;
        ArgumentNullException.ThrowIfNull(rootNode);
        (Version? version, Uri? uri) info = (null, null);

        var version = rootNode.GetAttributeValue("version");
        if (version is not null)
        {
            info.version = new Version(version);
        }

        var uri = rootNode.GetAttributeValue("version");
        if (uri is not null)
        {
            info.uri = new Uri(uri);
        }

        return new SpecificationInfo
        {
            Url = info.uri,
            Version = info.version
        };
    }

    private void RunParser(XmlNode node, string endpoint = "", int depth = 1)
    {
        if (depth > _maxDepth)
            throw new ConstraintException($"Maximum command depth of {_maxDepth} exceeded");

        var nodeNameAttr = node.GetAttributeValue("name");
        ArgumentNullException.ThrowIfNull(nodeNameAttr);
        endpoint += $"/{nodeNameAttr.Trim()}";

        if (node.HasChildNodes && ChildIsCommand(node))
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                RunParser(childNode, endpoint, ++depth);
            }
        }

        if (node.HasChildNodes && ChildIsParameter(node))
        {
            var request = new Request(endpoint, node.GetAttributeValue("description") ?? "");

            foreach (XmlNode childNode in node.ChildNodes)
            {
                var parameterName = childNode.GetAttributeValue("name");
                ArgumentNullException.ThrowIfNull(parameterName);

                request.AddParameter(new RequestParameter(
                    parameterName.Trim(),
                    childNode.GetAttributeValue("description") ?? "",
                    childNode.GetAttributeValue("allowed-values")?.Trim() ?? ""));
            }

            _requests.Add(request);
            return;
        }

        if (!node.HasChildNodes)
        {
            _requests.Add(new Request(endpoint, node.GetAttributeValue("description") ?? ""));
        }
    }

    private XmlDocument LoadXml(string path)
    {
        var doc = new XmlDocument();
        doc.Load(path);

        return doc;
    }

    private XmlNodeList GetRootChildrenNodes()
    {
        var nodes = _document.DocumentElement?.ChildNodes;
        ArgumentNullException.ThrowIfNull(nodes);

        return nodes;
    }

    private bool ChildIsParameter(XmlNode node) =>
        RegexExpr.ParameterNodeRegex().Match(node.FirstChild!.Name).Success;

    private bool ChildIsCommand(XmlNode node) =>
        RegexExpr.CommandNodeRegex().Match(node.FirstChild!.Name).Success;
}