using Npgsql;
using System.Data;

namespace PoundPupLegacy.Common;

public interface IDatabaseDeleter : IAsyncDisposable
{
    string Sql { get; }
    bool HasBeenPrepared { get; }

}
public interface IDatabaseDeleterFactory
{
}
public interface IDatabaseDeleterFactory<T>: IDatabaseDeleterFactory
    where T : IDatabaseDeleter
{
    public Task<T> CreateAsync(IDbConnection connection);
}
public abstract class DatabaseDeleter<TRequest> : IDatabaseDeleter
{
    protected NpgsqlCommand _command;
    protected DatabaseDeleter(NpgsqlCommand command)
    {
        _command = command;
    }
    public string Sql => _command.CommandText;
    public bool HasBeenPrepared => _command.IsPrepared;

    public abstract Task DeleteAsync(TRequest request);

    public virtual async ValueTask DisposeAsync()
    {
        await _command.DisposeAsync();
    }
}
