using Npgsql;
using System.Data;

namespace PoundPupLegacy.Common;

public interface IDatabaseUpdater : IAsyncDisposable
{
    string Sql { get; }
    bool HasBeenPrepared { get; }

}
public interface IDatabaseUpdaterFactory: IDatabaseAccessorFactory
{

}
public interface IDatabaseUpdaterFactory<T> : IDatabaseUpdaterFactory
    where T : IDatabaseUpdater
{
    Task<T> CreateAsync(IDbConnection connection);
    string Sql { get; }

}

public abstract class DatabaseUpdaterFactory<T> : DatabaseAccessorFactory, IDatabaseUpdaterFactory<T>
    where T : IDatabaseUpdater
{

    public async Task<T> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = Sql;

        foreach (var parameter in DatabaseParameters) {
            command.AddParameter(parameter);
        }
        await command.PrepareAsync();
        return (T)Activator.CreateInstance(typeof(T), new object[] { command })!;
    }

    public abstract string Sql { get; }

}
public abstract class DatabaseUpdater<TRequest> : DatabaseAccessor, IDatabaseUpdater
{
    protected DatabaseUpdater(NpgsqlCommand command) : base(command)
    {
    }
    public string Sql => _command.CommandText;
    public bool HasBeenPrepared => _command.IsPrepared;


    public async Task UpdateAsync(TRequest request)
    {
        foreach (var parameter in GetParameterValues(request)) 
        {
            parameter.Set(_command);
        }
        await _command.ExecuteNonQueryAsync();
    }

    public abstract IEnumerable<ParameterValue> GetParameterValues(TRequest request);

}
