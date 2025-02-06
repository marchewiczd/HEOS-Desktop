using Heos.Tools.ClassGenerator.Constants;
using Heos.Tools.XmlParser.Models;

namespace Heos.Tools.ClassGenerator.Generator;

internal class HeaderGenerator
{
    public string Get(string @namespace, SpecificationInfo specificationInfo, bool includeJsonUsings = false) =>
        ClassConstants.ClassHeader
            .Replace("{0}", GetSpecificationInfo(specificationInfo))
            .Replace("{1}", GetJsonUsings(includeJsonUsings))
            .Replace("{2}", @namespace);

    private string GetSpecificationInfo(SpecificationInfo? specificationInfo)
    {
        if (specificationInfo is null)
        {
            return string.Empty;
        }

        return $"\n// " +
               $"\n// Based on specification version: {specificationInfo.Version}" +
               $"\n// Specification address: {specificationInfo.Uri}";
    }

    private string GetJsonUsings(bool includeJsonUsings) =>
        includeJsonUsings ? "\nusing Newtonsoft.Json;\nusing Heos.API.JsonConverters;" : "";
}