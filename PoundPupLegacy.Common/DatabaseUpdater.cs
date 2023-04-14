using Npgsql;
using System.Data;

namespace PoundPupLegacy.Common;

public interface IDatabaseUpdater : IAsyncDisposable
{
    string Sql { get; }
    bool HasBeenPrepared { get; }

}

public interface IDatabaseUpdater<TRequest>: IDatabaseUpdater
    where TRequest: IRequest
{
    Task UpdateAsync(TRequest request);
}
public interface IDatabaseUpdaterFactory: IDatabaseAccessorFactory
{

}
public interface IDatabaseUpdaterFactory<TRequest> : IDatabaseUpdaterFactory
    where TRequest : IRequest
{
    Task<IDatabaseUpdater<TRequest>> CreateAsync(IDbConnection connection);
    string Sql { get; }

}

public abstract class DatabaseUpdaterFactory<TRequest, TUpdater> : DatabaseAccessorFactory, IDatabaseUpdaterFactory<TRequest>
    where TRequest : IRequest
    where TUpdater : IDatabaseUpdater<TRequest>
{

    public async Task<IDatabaseUpdater<TRequest>> CreateAsync(IDbConnection connection)
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
        return (IDatabaseUpdater<TRequest>)Activator.CreateInstance(typeof(TUpdater), new object[] { command })!;
    }

    public abstract string Sql { get; }

}
public abstract class DatabaseUpdater<TRequest> : DatabaseAccessor<TRequest>, IDatabaseUpdater<TRequest>
    where TRequest: IRequest
{
    protected DatabaseUpdater(NpgsqlCommand command) : base(command)
    {
    }

    public async Task UpdateAsync(TRequest request)
    {
        foreach (var parameter in GetParameterValues(request)) 
        {
            parameter.Set(_command);
        }
        await _command.ExecuteNonQueryAsync();
    }

}
