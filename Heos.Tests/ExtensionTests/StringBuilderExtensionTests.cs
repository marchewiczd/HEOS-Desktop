using System.Text;
using Heos.API.Extensions;

namespace Heos.Tests.ExtensionTests;

public class StringBuilderExtensionTests
{
    [Test]
    public void RemoveLast_ShouldRemoveLastFromString()
    {
        var expectedResult = "string";
        var stringBuilder = new StringBuilder($"{expectedResult}1");

        stringBuilder.RemoveLast();

        Assert.That(stringBuilder.ToString(), Is.EqualTo(expectedResult));
    }
}