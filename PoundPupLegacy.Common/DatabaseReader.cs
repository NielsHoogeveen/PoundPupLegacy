using Npgsql;
using System.Data;

namespace PoundPupLegacy.Common
{
    public interface IDatabaseReader : IAsyncDisposable
    {
        string Sql { get; }
        bool HasBeenPrepared { get; }
    }
    public interface IDatabaseReaderFactory { }
    public interface IDatabaseReaderFactory<T>: IDatabaseReaderFactory 
        where T : IDatabaseReader
    {
        public Task<T> CreateAsync(IDbConnection connection);
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
        public string Sql => _command.CommandText;
        public bool HasBeenPrepared => _command.IsPrepared;
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
