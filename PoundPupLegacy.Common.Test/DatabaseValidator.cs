using Npgsql;
using System.Reflection;

namespace PoundPupLegacy.Common.Test;

public class DatabaseValidator : DatabaseValidatorBase
{
    protected override Type GetClassTypeForInterface(Type interfaceType)
    {
        var types = interfaceType.Assembly!.GetTypes().Where(x => x.IsAssignableTo(interfaceType) && !x.IsInterface && !x.IsAbstract && !x.IsGenericType);
        Assert.True(types.Any());
        if (types.Any()) {
            return types.First();
        }
        throw new Exception("Shouldn't get here");
    }
}

public abstract class DatabaseValidatorBase
{
    const string ConnectStringPostgresql = "Host=localhost;Username=niels;Password=niels;Database=ppl;Include Error Detail=True";

    private static Type? GetRequestType(Type t)
    {
        var genericArguments = t.GetGenericArguments();
        if (genericArguments.Any()) {
            foreach (var genericArgument in genericArguments) {
                if (genericArgument.IsAssignableTo(typeof(IRequest))) {
                    return genericArgument;
                }
            }
        }
        if (t.BaseType is not null) {
            var result = GetRequestType(t.BaseType);
            if (result is not null)
                return result;
        }
        return null;
    }

    protected abstract Type GetClassTypeForInterface(Type interfaceType);

    private static void CheckDatabaseParameters(IDatabaseAccessorFactory factory, IDatabaseAccessor accessor, Type requestType)
    {
        var databaseParameters = factory.DatabaseParameters;
        var request = Activator.CreateInstance(requestType);
        Assert.NotNull(request);
        var method = accessor.GetType().GetMethod("GetParameterValues", BindingFlags.NonPublic | BindingFlags.Instance, new Type[] { request.GetType() });
        Assert.NotNull(method);
        var parameterValues = method.Invoke(accessor, new object[] { request }) as IEnumerable<ParameterValue>;
        Assert.NotNull(parameterValues);
        var usedDatabaseParameters = parameterValues.Select(x => x.DatabaseParameter);
        if (!usedDatabaseParameters.All(x => databaseParameters.Contains(x))) {
            Console.WriteLine(""); ;
        }
        Assert.True(usedDatabaseParameters.All(x => databaseParameters.Contains(x)));
        Assert.True(databaseParameters.All(x => usedDatabaseParameters.Contains(x)));
        Assert.Equal(usedDatabaseParameters.Count(), databaseParameters.Count());

    }

    public async Task ValidateDatabaseAccessors(Type t)
    {
        using var connection = new NpgsqlConnection(ConnectStringPostgresql);
        connection.Open();
        var creatorAssembly = Assembly.GetAssembly(t);
        var types = creatorAssembly!.GetTypes().Where(x => x.IsAssignableTo(typeof(IDatabaseAccessorFactory)) && !x.IsInterface && !x.IsAbstract && !x.IsGenericType);
        bool foundTypes = false;
        foreach (var type in types) {
            foundTypes = true;
            var factory = (IDatabaseAccessorFactory)Activator.CreateInstance(type)!;
            var createMethod = type.GetMethod("CreateAsync", new Type[] { typeof(NpgsqlConnection) });
            var task = (Task)createMethod!.Invoke(factory, new object[] { connection })!;
            await task.ConfigureAwait(false);
            var result = task.GetType().GetProperty("Result");
            Assert.NotNull(result);
            var accessor = result.GetValue(task) as IDatabaseAccessor;
            Assert.NotNull(accessor);
            Assert.True(accessor.HasBeenPrepared);
            Assert.NotEqual(string.Empty, accessor.Sql);
            await accessor.DisposeAsync();
            var requestType = GetRequestType(accessor.GetType());
            Assert.NotNull(requestType);
            if (requestType.IsClass && !requestType.IsAbstract) {
                CheckDatabaseParameters(factory, accessor, requestType);
            }
            if (requestType.IsInterface) {
                CheckDatabaseParameters(factory, accessor, GetClassTypeForInterface(requestType));
            }
        }
        Assert.True(foundTypes);
        connection.Close();

    }
}