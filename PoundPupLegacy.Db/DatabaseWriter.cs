using Npgsql;

namespace PoundPupLegacy.Db;

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

    public abstract void Write(T item);
}
