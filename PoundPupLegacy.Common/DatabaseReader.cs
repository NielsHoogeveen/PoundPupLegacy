using Npgsql;

namespace PoundPupLegacy.Common
{
    public interface IDatabaseReader : IAsyncDisposable
    {

    }
    public interface IDatabaseReaderFactory<T>
        where T : IDatabaseReader
    {
        public Task<T> CreateAsync(NpgsqlConnection connection);
    }

    public interface ISingleItemDatabaseReader<TRequest, TResponse> : IDatabaseReader
    {
        public Task<TResponse> ReadAsync(TRequest request);
    }
    public interface IEnumerableDatabaseReader<TRequest, TResponse>
    {
        public IAsyncEnumerable<TResponse> ReadAsync(TRequest request);
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

    public abstract class SingleItemDatabaseReader<TRequest, TResponse> : DatabaseReader, ISingleItemDatabaseReader<TRequest, TResponse>
    {
        protected SingleItemDatabaseReader(NpgsqlCommand command) : base(command)
        {
        }
        public abstract Task<TResponse> ReadAsync(TRequest request);
    }
    public abstract class EnumerableDatabaseReader<TRequest, TResponse> : DatabaseReader, IEnumerableDatabaseReader<TRequest, TResponse>
    {
        protected EnumerableDatabaseReader(NpgsqlCommand command) : base(command)
        {
        }
        public abstract IAsyncEnumerable<TResponse> ReadAsync(TRequest request);
    }

}
