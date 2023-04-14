using Npgsql;
using System.Data;

namespace PoundPupLegacy.Common
{
    public interface IDatabaseReaderFactory {
        string Sql { get; }
    }
    public interface IDatabaseReaderFactory<TRequest, TResponse> : IDatabaseReaderFactory
        where TRequest : IRequest
    {
        
    }

    public interface IDatabaseReader: IDatabaseAccessor 
    { 
        
    }
    public interface IDatabaseReader<TRequest, TResponse> : IDatabaseReader
        where TRequest : IRequest
    {

    }

    public interface ISingleItemDatabaseReader<TRequest, TResponse> : IDatabaseReader<TRequest, TResponse>
        where TRequest: IRequest
    {
        public Task<TResponse?> ReadAsync(TRequest request);
    }
    public interface IMandatorySingleItemDatabaseReader<TRequest, TResponse> : IDatabaseReader<TRequest, TResponse>
    where TRequest : IRequest
    {
        public Task<TResponse> ReadAsync(TRequest request);
    }

    public interface IEnumerableDatabaseReader<TRequest, TResponse>: IDatabaseReader<TRequest, TResponse>
        where TRequest: IRequest
    {
        public IAsyncEnumerable<TResponse> ReadAsync(TRequest request);
    }

    public interface ISingleItemDatabaseReaderFactory<TRequest, TResponse> : IDatabaseReaderFactory<TRequest, TResponse>
        where TRequest: IRequest
    {
        public Task<ISingleItemDatabaseReader<TRequest, TResponse>> CreateAsync(IDbConnection connection);
    }

    public abstract class SingleItemDatabaseReaderFactory<TRequest, TResponse, TReader>: DatabaseReaderFactory<TRequest, TResponse>, ISingleItemDatabaseReaderFactory<TRequest, TResponse>
        where TRequest: IRequest
        where TReader : ISingleItemDatabaseReader<TRequest, TResponse>
    {
        public async Task<ISingleItemDatabaseReader<TRequest, TResponse>> CreateAsync(IDbConnection connection)
        {
            return (TReader)Activator.CreateInstance(typeof(TReader), new object[] { await GetCommand(connection) })!;
        }

    }
    public interface IMandatorySingleItemDatabaseReaderFactory<TRequest, TResponse>: IDatabaseReaderFactory<TRequest, TResponse> 
        where TRequest: IRequest
    {
        public Task<IMandatorySingleItemDatabaseReader<TRequest, TResponse>> CreateAsync(IDbConnection connection);
    }
    public abstract class MandatorySingleItemDatabaseReaderFactory<TRequest, TResponse, TReader> : DatabaseReaderFactory<TRequest, TResponse>, IMandatorySingleItemDatabaseReaderFactory<TRequest, TResponse>
        where TRequest : IRequest
        where TReader : IMandatorySingleItemDatabaseReader<TRequest, TResponse>
    {
        public async Task<IMandatorySingleItemDatabaseReader<TRequest, TResponse>> CreateAsync(IDbConnection connection)
        {
            return (TReader)Activator.CreateInstance(typeof(TReader), new object[] { await GetCommand(connection) })!;
        }

    }
    public interface IEnumerableDatabaseReaderFactory<TRequest, TResponse> : IDatabaseReaderFactory<TRequest, TResponse>
    where TRequest : IRequest
    {
        public Task<IEnumerableDatabaseReader<TRequest, TResponse>> CreateAsync(IDbConnection connection);
    }

    public abstract class EnumerableDatabaseReaderFactory<TRequest, TResponse, TReader> : DatabaseReaderFactory<TRequest, TResponse>, IEnumerableDatabaseReaderFactory<TRequest, TResponse>
        where TRequest : IRequest
        where TReader : IEnumerableDatabaseReader<TRequest, TResponse>
    {
        public async Task<IEnumerableDatabaseReader<TRequest, TResponse>> CreateAsync(IDbConnection connection)
        {
            return (TReader)Activator.CreateInstance(typeof(TReader), new object[] { await GetCommand(connection) })!;
        }

    }

    public abstract class DatabaseReaderFactory<TRequest, TResponse> : DatabaseAccessorFactory, IDatabaseReaderFactory<TRequest, TResponse>
        where TRequest: IRequest
    {
        public abstract string Sql { get; }

        protected async Task<NpgsqlCommand> GetCommand(IDbConnection connection)
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
            return command;
        }

    }


    public abstract class IntDatabaseReader<TRequest> : MandatorySingleItemDatabaseReader<TRequest, int>
        where TRequest : IRequest
    {
        protected IntDatabaseReader(NpgsqlCommand command) : base(command)
        {
        }

        protected abstract IntValueReader IntValueReader { get; }


        protected sealed override int Read(NpgsqlDataReader reader)
        {
            return IntValueReader.GetValue(reader);
        }
    }
    public abstract class MandatorySingleItemDatabaseReader<TRequest, TResponse> : DatabaseAccessor<TRequest>, IMandatorySingleItemDatabaseReader<TRequest, TResponse>
        where TRequest : IRequest
    {
        protected MandatorySingleItemDatabaseReader(NpgsqlCommand command) : base(command)
        {
        }

        protected abstract string GetErrorMessage(TRequest request);

        protected abstract TResponse Read(NpgsqlDataReader reader);


        public async Task<TResponse> ReadAsync(TRequest request)
        {
            foreach (var parameter in GetParameterValues(request)) 
            {
                parameter.Set(_command);
            }
            await using var reader = await _command.ExecuteReaderAsync();
            if (!reader.HasRows)
                throw new Exception(GetErrorMessage(request)); ;
            await reader.ReadAsync();
            return Read(reader);
        }
    }

    public abstract class SingleItemDatabaseReader<TRequest, TResponse> : DatabaseAccessor<TRequest>, ISingleItemDatabaseReader<TRequest, TResponse>
        where TResponse : class
        where TRequest : IRequest
    {
        protected SingleItemDatabaseReader(NpgsqlCommand command) : base(command)
        {
        }
        protected abstract TResponse? Read(NpgsqlDataReader reader);

        public async Task<TResponse?> ReadAsync(TRequest request)
        {
            foreach (var parameter in GetParameterValues(request)) 
            { 
                parameter.Set(_command);
            }
            await using var reader = await _command.ExecuteReaderAsync();
            if (!reader.HasRows)
                return null;
            await reader.ReadAsync();
            return Read(reader);
        }
    }
    public abstract class EnumerableDatabaseReader<TRequest, TResponse> : DatabaseAccessor<TRequest>, IEnumerableDatabaseReader<TRequest, TResponse>
        where TRequest: IRequest
    {
        protected EnumerableDatabaseReader(NpgsqlCommand command) : base(command)
        {
        }
        protected abstract TResponse Read(NpgsqlDataReader reader);

        public async IAsyncEnumerable<TResponse> ReadAsync(TRequest request)
        {
            foreach (var parameter in GetParameterValues(request)) 
            {
                parameter.Set(_command);
            }
            await using var reader = await _command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) {
                yield return Read(reader);
            }
        }
    }
    public abstract record class ValueReader<T>
    {
        public required string Name { get; init; }

        public abstract T GetValue(NpgsqlDataReader reader);
    }
    public record class FieldValueReader<T> : ValueReader<T>
    {
        public override T GetValue(NpgsqlDataReader reader)
        {
            return reader.GetFieldValue<T>(reader.GetOrdinal(Name));
        }
    }

    public record class IntValueReader : ValueReader<int>
    {
        public override int GetValue(NpgsqlDataReader reader)
        {
            return reader.GetInt32(reader.GetOrdinal(Name));
        }
    }
    public record class NullableIntValueReader : ValueReader<int?>
    {
        public override int? GetValue(NpgsqlDataReader reader)
        {
            var ordinal = reader.GetOrdinal(Name);
            if(reader.IsDBNull(ordinal))
                return null;
            return reader.GetInt32(ordinal);
        }
    }
    public record class LongValueReader : ValueReader<long>
    {
        public override long GetValue(NpgsqlDataReader reader)
        {
            return reader.GetInt64(reader.GetOrdinal(Name));
        }
    }
    public record class NullableLongValueReader : ValueReader<long?>
    {
        public override long? GetValue(NpgsqlDataReader reader)
        {
            var ordinal = reader.GetOrdinal(Name);
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetInt64(ordinal);
        }
    }
    public record class StringValueReader : ValueReader<string>
    {
        public override string GetValue(NpgsqlDataReader reader)
        {
            return reader.GetString(reader.GetOrdinal(Name));
        }
    }
    public record class NullableStringValueReader : ValueReader<string?>
    {
        public override string? GetValue(NpgsqlDataReader reader)
        {
            var ordinal = reader.GetOrdinal(Name);
            if(reader.IsDBNull(ordinal))
                return null;
            else 
                return reader.GetString(ordinal);
        }
    }

    public record class DateTimeValueReader : ValueReader<DateTime>
    {
        public override DateTime GetValue(NpgsqlDataReader reader)
        {
            return reader.GetDateTime(reader.GetOrdinal(Name));
        }
    }
    public record class NullableDateTimeValueReader : ValueReader<DateTime?>
    {
        public override DateTime? GetValue(NpgsqlDataReader reader)
        {
            var ordinal = (int)reader.GetOrdinal(Name);
            if(reader.IsDBNull(ordinal))
                return null;
            return reader.GetDateTime(ordinal);
        }
    }
    public record class BooleanValueReader : ValueReader<bool>
    {
        public override bool GetValue(NpgsqlDataReader reader)
        {
            return reader.GetBoolean(reader.GetOrdinal(Name));
        }
    }
    public record class NullanleBooleanValueReader : ValueReader<bool?>
    {
        public override bool? GetValue(NpgsqlDataReader reader)
        {
            var ordinal = reader.GetOrdinal(Name);
            if(reader.IsDBNull(ordinal))
                return null;
            return reader.GetBoolean(ordinal);
        }
    }
}
