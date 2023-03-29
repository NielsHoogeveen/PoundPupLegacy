using Npgsql;
namespace PoundPupLegacy.Common;

public interface IDatabaseDeleter : IAsyncDisposable
{
}
public interface IDatabaseDeleterFactory<T>
    where T : IDatabaseDeleter
{
    public Task<T> CreateAsync(NpgsqlConnection connection);
}
public abstract class DatabaseDeleter<TRequest> : IDatabaseDeleter
{
    protected NpgsqlCommand _command;
    protected DatabaseDeleter(NpgsqlCommand command)
    {
        _command = command;
    }

    public abstract Task DeleteAsync(TRequest request);


    public virtual async ValueTask DisposeAsync()
    {
        await _command.DisposeAsync();
    }
}
