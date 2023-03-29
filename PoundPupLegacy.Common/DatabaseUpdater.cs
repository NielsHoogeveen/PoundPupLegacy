using Npgsql;
namespace PoundPupLegacy.Common;

public interface IDatabaseUpdater : IAsyncDisposable
{
}
public interface IDatabaseUpdaterFactory<T>
    where T : IDatabaseUpdater
{
    public Task<T> CreateAsync(NpgsqlConnection connection);
}
public abstract class DatabaseUpdater<TRequest> : IDatabaseUpdater
{
    protected NpgsqlCommand _command;
    protected DatabaseUpdater(NpgsqlCommand command)
    {
        _command = command;
    }

    public abstract Task Update(TRequest request);


    public virtual async ValueTask DisposeAsync()
    {
        await _command.DisposeAsync();
    }
}
