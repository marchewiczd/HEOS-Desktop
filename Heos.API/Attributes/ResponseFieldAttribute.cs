using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Heos.API.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ResponseFieldAttribute : Attribute
{
    public ResponseFieldAttribute() : this(string.Empty)
    {
        
    }
    
    public ResponseFieldAttribute(string description)
    {
        Description = description;
    }
    
    public string Description { get; init; }
    
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is DescriptionAttribute other && other.Description == Description;

    public override int GetHashCode() => Description?.GetHashCode() ?? 0;
}