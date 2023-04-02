namespace PoundPupLegacy.CreateModel.Updaters;

public interface IDatabaseUpdater : IAsyncDisposable
{

}
public interface IDatabaseUpdater<T> : IDatabaseUpdater
    where T : IDatabaseUpdater<T>
{
    public abstract static Task<T> CreateAsync(IDbConnection connection);
}
public abstract class DatabaseUpdater<T> : IDatabaseUpdater
{
    protected NpgsqlCommand _command;
    protected DatabaseUpdater(NpgsqlCommand command)
    {
        _command = command;

    }
    public virtual async ValueTask DisposeAsync()
    {
        await _command.DisposeAsync();
    }

}
