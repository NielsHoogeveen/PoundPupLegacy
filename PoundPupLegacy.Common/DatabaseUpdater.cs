using Npgsql;
using System.Data;

namespace PoundPupLegacy.Common;

public interface IDatabaseUpdater : IAsyncDisposable
{
    string Sql { get; }
    bool HasBeenPrepared { get; }

}
public interface IDatabaseUpdaterFactory
{

}
public interface IDatabaseUpdaterFactory<T>: IDatabaseUpdaterFactory
    where T : IDatabaseUpdater
{
    public Task<T> CreateAsync(IDbConnection connection);
}
public abstract class DatabaseUpdater<TRequest> : IDatabaseUpdater
{
    protected NpgsqlCommand _command;
    protected DatabaseUpdater(NpgsqlCommand command)
    {
        _command = command;
    }
    public string Sql => _command.CommandText;
    public bool HasBeenPrepared => _command.IsPrepared;

    public abstract Task UpdateAsync(TRequest request);

    public virtual async ValueTask DisposeAsync()
    {
        await _command.DisposeAsync();
    }
}
