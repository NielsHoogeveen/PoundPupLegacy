using Npgsql;
using System.Data;

namespace PoundPupLegacy.Common
{
    public interface IDatabaseReaderFactory {
        string Sql { get; }
    }
    public interface IDatabaseReaderFactory<T> : IDatabaseReaderFactory
        where T : IDatabaseReader
    {
        public Task<T> CreateAsync(IDbConnection connection);
    }

    public interface IDatabaseReader: IDatabaseAccessor 
    { 
        
    }

    public interface ISingleItemDatabaseReader<TRequest, TResponse> : IDatabaseReader
    {
        public Task<TResponse> ReadAsync(TRequest request);
    }
    public interface IEnumerableDatabaseReader<TRequest, TResponse>: IDatabaseReader
    {
        public IAsyncEnumerable<TResponse> ReadAsync(TRequest request);
    }

    public abstract class DatabaseReaderFactory<T> : DatabaseAccessorFactory, IDatabaseReaderFactory<T>
        where T : IDatabaseReader
    {
        public abstract string Sql { get; }

        public async Task<T> CreateAsync(IDbConnection connection)
        {
            if (connection is not NpgsqlConnection)
                throw new Exception("Application only works with a Postgres database");
            var postgresConnection = (NpgsqlConnection)connection;
            var command = postgresConnection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 300;
            command.CommandText = Sql;
            foreach (var parameter in DatabaseParameters) {
                command.AddParameter(parameter);
            }
            await command.PrepareAsync();
            return (T)Activator.CreateInstance(typeof(T), new object[] { command })!;
        }


    }

    public abstract class DatabaseReader : DatabaseAccessor, IDatabaseReader
    {
        protected DatabaseReader(NpgsqlCommand command): base(command)
        {
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
