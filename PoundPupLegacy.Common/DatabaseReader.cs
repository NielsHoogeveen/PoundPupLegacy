using Npgsql;

namespace PoundPupLegacy.Common
{
    public interface IDatabaseReader : IAsyncDisposable
    {

    }
    public interface IDatabaseReader<T> : IDatabaseReader
        where T : IDatabaseReader<T>
    {
        public abstract static Task<T> CreateAsync(NpgsqlConnection connection);
    }
    public abstract class DatabaseReader : IDatabaseReader
    {
        protected NpgsqlCommand _command;
        protected DatabaseReader(NpgsqlCommand command)
        {
            _command = command;
        }
        public virtual async ValueTask DisposeAsync()
        {
            await _command.DisposeAsync();
        }

    }
}
