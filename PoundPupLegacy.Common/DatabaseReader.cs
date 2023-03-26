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

    public interface ISingleItemDatabaseReader<TReader, TRequest, TResponse>: IDatabaseReader<TReader>
        where TReader : IDatabaseReader<TReader>
    {
        public Task<TResponse> ReadAsync(TRequest request);
    }
    public interface IEnumerableDatabaseReader<TReader, TRequest, TResponse> : IDatabaseReader<TReader>
        where TReader : IDatabaseReader<TReader>
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
}
