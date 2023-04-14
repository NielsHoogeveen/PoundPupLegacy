using Npgsql;

namespace PoundPupLegacy.Common;

public interface IDatabaseAccessorFactory
{
    IEnumerable<DatabaseParameter> DatabaseParameters { get; }
}
public abstract class DatabaseAccessorFactory : IDatabaseAccessorFactory
{
    public IEnumerable<DatabaseParameter> DatabaseParameters => GetDatabaseParameters();
    private List<DatabaseParameter> GetDatabaseParameters()
    {
        var t = GetType();
        var fields = GetStaticNonPublicFields(t);
        return fields.Select(x => x.GetValue(null) as DatabaseParameter).Where(x => x is not null).Select(x => (DatabaseParameter)x!).ToList();
    }

    private IEnumerable<System.Reflection.FieldInfo> GetStaticNonPublicFields(Type t)
    {
        foreach(var elem in t.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)) {
            yield return elem;
        }
        if(t.BaseType is not null) {

            foreach(var elem in GetStaticNonPublicFields((Type)t.BaseType)) {
                yield return elem;
            }
        }
    }

}

public interface IDatabaseAccessor : IAsyncDisposable
{
    string Sql { get; }
    bool HasBeenPrepared { get; }
}

public abstract class DatabaseAccessor<TRequest>: IDatabaseAccessor
    where TRequest: IRequest
{
    protected readonly NpgsqlCommand _command;
    public string Sql => _command.CommandText;
    public bool HasBeenPrepared => _command.IsPrepared;

    protected DatabaseAccessor(NpgsqlCommand command)
    {
        _command = command;
    }
    public async virtual ValueTask DisposeAsync()
    {
        await _command.DisposeAsync();
    }
    protected abstract IEnumerable<ParameterValue> GetParameterValues(TRequest request);

}
