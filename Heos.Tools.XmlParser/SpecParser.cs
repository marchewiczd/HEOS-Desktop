using System.Data;
using System.Xml;
using Heos.Tools.XmlParser.Constants;
using Heos.Tools.XmlParser.Extensions;
using Heos.Tools.XmlParser.Models;

namespace Heos.Tools.XmlParser;

public class SpecParser
{
    private readonly List<Command> _requests = [];
    private readonly int _maxDepth;
    private readonly string _filePath;
    private XmlDocument? _document;

    public SpecParser(string filePath, int maxDepth = 5)
    {
        _filePath = filePath;
        _maxDepth = maxDepth;
    }

    public IEnumerable<Command> Parse()
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
        var rootNode = _document?.DocumentElement;
        ArgumentNullException.ThrowIfNull(rootNode);
        (Version? version, Uri? uri) info = (null, null);

        var version = rootNode.GetAttributeValue(AttributeName.Version);
        if (version is not null)
        {
            info.version = new Version(version);
        }

        var uri = rootNode.GetAttributeValue(AttributeName.Uri);
        if (uri is not null)
        {
            info.uri = new Uri(uri);
        }

        return new SpecificationInfo
        {
            Uri = info.uri,
            Version = info.version
        };
    }

    private void RunParser(XmlNode node, string endpoint = "", int depth = 1)
    {
        if (depth > _maxDepth)
            throw new ConstraintException($"Maximum command depth of {_maxDepth} exceeded");

        var nodeNameAttr = node.GetAttributeValue(AttributeName.Name);
        ArgumentNullException.ThrowIfNull(nodeNameAttr);
        endpoint += $"/{nodeNameAttr.Trim()}";

        if (node.HasChildNodes)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (IsCommand(childNode))
                {
                    RunParser(childNode, endpoint, depth++);

                    //TODO: fix depth count
                    depth--;
                }

                if (IsRequest(childNode))
                {
                    var request = FindOrNew(endpoint,
                        node.GetAttributeValue(AttributeName.Description) ?? "");

                    foreach (XmlNode parameterNode in childNode.ChildNodes)
                    {
                        var parameterName = parameterNode.GetAttributeValue(AttributeName.Name);
                        ArgumentNullException.ThrowIfNull(parameterName);

                        request.AddParameter(new RequestParameter(
                            parameterName.Trim(),
                            parameterNode.GetAttributeValue(AttributeName.Description) ?? "",
                            parameterNode.GetAttributeValue("allowed-values")?.Trim() ?? ""));
                    }
                }

                if (IsResponse(childNode))
                {
                    var request = FindOrNew(endpoint,
                        node.GetAttributeValue(AttributeName.Description) ?? "");

                    var responseType = childNode.GetAttributeValue(AttributeName.ResponseType);
                    ArgumentNullException.ThrowIfNull(responseType);

                    request.ResponseType = responseType;

                    foreach (XmlNode responseFieldType in childNode.ChildNodes)
                    {
                        var parameterName = responseFieldType.GetAttributeValue(AttributeName.Name);
                        ArgumentNullException.ThrowIfNull(parameterName);

                        request.AddResponseField(new ResponseField(parameterName.Trim()));
                    }
                }

            }
        }

        if (!node.HasChildNodes)
        {
            _requests.Add(new Command(endpoint, node.GetAttributeValue(AttributeName.Description) ?? ""));
        }
    }

    private XmlDocument LoadXml(string path)
    {
        var readerSettings = new XmlReaderSettings();
        readerSettings.IgnoreComments = true;

        var reader = XmlReader.Create(path, readerSettings);
        var doc = new XmlDocument();

        doc.Load(reader);
        reader.Dispose();

        return doc;
    }

    private XmlNodeList GetRootChildrenNodes()
    {
        var nodes = _document?.DocumentElement?.ChildNodes;
        ArgumentNullException.ThrowIfNull(nodes);

        return nodes;
    }

    private bool IsRequest(XmlNode node) =>
        RegexExpr.RequestNodeRegex().Match(node.Name).Success;

    private bool IsCommand(XmlNode node) =>
        RegexExpr.CommandNodeRegex().Match(node.Name).Success;

    private bool IsResponse(XmlNode node) =>
        RegexExpr.ResponseNodeRegex().Match(node.Name).Success;

    private Command FindOrNew(string endpoint, string description)
    {
        var result = _requests.FirstOrDefault(x =>
            x.Endpoint.Equals(endpoint) && x.Description.Equals(description));

        if (result is not null)
            return result;

        result = new Command(endpoint, description);
        _requests.Add(result);

        return result;
    }
}