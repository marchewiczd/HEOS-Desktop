using System.Reflection;
using Heos.API.Models.HEOS;
using Heos.Common.Extensions;
using Heos.Console.Extensions;

namespace Heos.Console.Helpers;

public class RequestMapper
{
    public HeosRequest CreateRequestInstance(string requestName, params string[] parameters)
    {
        var commandName = GetRequestName(requestName);
        var requestType = GetHeosRequestType(commandName);
        var commandObject = (HeosRequest?)Activator.CreateInstance(requestType, parameters.ToObjectArray());

        ArgumentNullException.ThrowIfNull(commandObject);

        return commandObject;
    }

    public IEnumerable<Type> GetAll()
    {
        var types = Assembly.GetAssembly(typeof(HeosRequest))?.GetTypes()
            .Where(myType => myType is { IsClass: true, IsAbstract: false }
                             && myType.IsSubclassOf(typeof(HeosRequest)));

        ArgumentNullException.ThrowIfNull(types);

        return types;
    }

    private Type GetHeosRequestType(string commandName) =>
        GetAll().First(x => x.Name.Equals(commandName, StringComparison.InvariantCultureIgnoreCase));

    private string GetRequestName(string command)
    {
        var name = command.Contains('_') ? command.SnakeCaseToPascalCase() : command;
        return $"{name}Request";
    }
}