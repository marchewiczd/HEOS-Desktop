using System.Data;
using System.Xml;
using Heos.Tools.RCG.Constants;
using Heos.Tools.RCG.Extensions;
using Heos.Tools.RCG.Models;

namespace Heos.Tools.RCG.Helpers;

public class XmlParser
{
    private readonly List<Request> _requests = new();
    private int _maxDepth;
    private string _filePath;

    public XmlParser(string filePath, int maxDepth)
    {
        _filePath = filePath;
        _maxDepth = maxDepth;
    }

    public XmlParser() : this("RequestSpecification.xml", 5) { }

    public IEnumerable<Request> Parse()
    {
        _requests.Clear();
        var xml = LoadXml(_filePath);
        var childrenOfRoot = GetRootChildrenNodes(xml);

        foreach (XmlNode node in childrenOfRoot)
        {
            RunParser(node);
        }

        return _requests;
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

    private XmlNodeList GetRootChildrenNodes(XmlDocument document)
    {
        var nodes = document.DocumentElement?.ChildNodes;
        ArgumentNullException.ThrowIfNull(nodes);

        return nodes;
    }

    private bool ChildIsParameter(XmlNode node) =>
        RegexExpr.ParameterNodeRegex().Match(node.FirstChild!.Name).Success;

    private bool ChildIsCommand(XmlNode node) =>
        RegexExpr.CommandNodeRegex().Match(node.FirstChild!.Name).Success;
}