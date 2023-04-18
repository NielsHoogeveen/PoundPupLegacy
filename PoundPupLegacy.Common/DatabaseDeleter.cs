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

public interface IDatabaseDeleterFactory: IDatabaseAccessorFactory
{
}
public interface IDatabaseDeleterFactory<TRequest> : IDatabaseDeleterFactory
    where TRequest : IRequest
{
    public Task<IDatabaseDeleter<TRequest>> CreateAsync(IDbConnection connection);
}

public abstract class DatabaseDeleterFactory<TRequest> : DatabaseAccessorFactory, IDatabaseDeleterFactory<TRequest>
    where TRequest : IRequest
{

    protected abstract IEnumerable<ParameterValue> GetParameterValues(TRequest request);
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
        return new DatabaseDeleter<TRequest>(command, GetParameterValues);
    }
    public abstract string Sql { get; }
}
public class DatabaseDeleter<TRequest> : DatabaseAccessor<TRequest>, IDatabaseDeleter<TRequest>
    where TRequest: IRequest
{
    private readonly Func<TRequest, IEnumerable<ParameterValue>> _parameterMapper;
    public DatabaseDeleter(NpgsqlCommand command, Func<TRequest, IEnumerable<ParameterValue>> parameterMapper) : base(command)
    {
        _parameterMapper = parameterMapper;
    }
    protected sealed override IEnumerable<ParameterValue> GetParameterValues(TRequest request)
    {
        return _parameterMapper(request);
    }

    public async Task DeleteAsync(TRequest request)
    {
        foreach (var parameter in GetParameterValues(request)) {
            parameter.Set(_command);
        }
        await _command.ExecuteNonQueryAsync();
    }
}
