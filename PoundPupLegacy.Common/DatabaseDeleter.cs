using Npgsql;
using System.Data;

namespace PoundPupLegacy.Common;

public interface IDatabaseDeleter : IAsyncDisposable
{
    string Sql { get; }
    bool HasBeenPrepared { get; }

}

public interface IDatabaseDeleter<TRequest>: IDatabaseDeleter
    where TRequest: IRequest
{
    Task DeleteAsync(TRequest request);
}

public interface IDatabaseDeleterFactory
{
}
public interface IDatabaseDeleterFactory<TRequest> : IDatabaseDeleterFactory
    where TRequest : IRequest
{
    public Task<IDatabaseDeleter<TRequest>> CreateAsync(IDbConnection connection);
}

public abstract class DatabaseDeleterFactory<TRequest, TDeleter> : DatabaseAccessorFactory, IDatabaseDeleterFactory<TRequest>
    where TRequest : IRequest
    where TDeleter : IDatabaseDeleter<TRequest>
{

    public async Task<IDatabaseDeleter<TRequest>> CreateAsync(IDbConnection connection)
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
        return (IDatabaseDeleter<TRequest>)Activator.CreateInstance(typeof(TDeleter), new object[] { command })!;
    }
    public abstract string Sql { get; }
}
public abstract class DatabaseDeleter<TRequest> : DatabaseAccessor<TRequest>, IDatabaseDeleter<TRequest>
    where TRequest: IRequest
{
    protected DatabaseDeleter(NpgsqlCommand command): base(command)
    {
    }

    public async Task DeleteAsync(TRequest request)
    {
        foreach (var parameter in GetParameterValues(request)) {
            parameter.Set(_command);
        }
        await _command.ExecuteNonQueryAsync();
    }

}
