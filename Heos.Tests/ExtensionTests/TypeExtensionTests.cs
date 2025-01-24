using Heos.API.Attributes;
using Heos.API.Extensions;

namespace Heos.Tests.ExtensionTests;

public class TypeExtensionTests
{
    [Test]
    public void GetPropertiesWithAttribute_ReturnsOnlyCorrectProperties()
    {
        string[] expected = ["Bar1", "Bar2"];
        var foo = new Foo();
        var props =
            foo.GetType().GetPropertiesWithAttribute(typeof(ResponseFieldAttribute));

        var result = props.Select(info => info.Name).ToArray();

        Assert.That(expected, Is.EquivalentTo(result));
    }
}

internal class Foo
{
    [ResponseField] public object Bar1 { get; set; } = null!;
    [ResponseField] public object Bar2 { get; set; } = null!;
    public object Bar3 { get; set; } = null!;
    public object Bar4 { get; set; } = null!;
}