using System.Reflection;
using Heos.API.Models.HEOS;
using Heos.Common.Extensions;
using Heos.Console.Configuration;
using Heos.Console.Extensions;
using Heos.Console.Model;

namespace Heos.Console.Helpers;

public class RequestMapper
{
    public HeosRequest CreateRequestInstance(CliArguments cliArguments)
    {
        var requestName = cliArguments.Command;
        var commandName = GetRequestName(requestName);
        var requestType = GetHeosRequestType(commandName);
        var commandObject = (HeosRequest?)Activator.CreateInstance(requestType, GetCtorParams(cliArguments, requestType));

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

    private int GetPidParameterIndex(Type requestType)
    {
        var ctorParams = requestType.GetConstructors().First().GetParameters().ToList();
        return ctorParams.FindIndex(param => param.Name is not null && param.Name.Equals("pid"));
    }

    private object[]? GetCtorParams(CliArguments cliArguments, Type requestType)
    {
        var pidIndex = GetPidParameterIndex(requestType);
        if (pidIndex == -1)
        {
            return null;
        }

        cliArguments.Parameters.Insert(pidIndex, cliArguments.Pid);

        return cliArguments.Parameters.ToArray<object>();
    }
}