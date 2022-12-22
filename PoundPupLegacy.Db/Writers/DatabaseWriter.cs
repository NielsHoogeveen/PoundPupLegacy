using Npgsql;

namespace PoundPupLegacy.Db.Writers;

public interface IDatabaseWriter<T>
{
    public abstract static DatabaseWriter<T> Create(NpgsqlConnection connection);
}

public abstract class DatabaseWriter<T> : IDisposable
{
    protected readonly NpgsqlCommand _command;

    protected DatabaseWriter(NpgsqlCommand command)
    {
        _command = command;
    }

    public void Dispose()
    {
        _command.Dispose();
    }

    internal abstract void Write(T item);
}
