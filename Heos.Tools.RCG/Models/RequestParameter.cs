namespace Heos.Tools.RCG.Models;

public class RequestParameter
{
    public string Name { get; init; }
    public string Value { get; init; }
    public string Description { get; init; }
    private readonly List<string> _allowedValues;

    public RequestParameter(string name, string description, string allowedValues)
    {
        Name = name;
        Description = description;
        _allowedValues = allowedValues.Split(",").ToList();
    }
}