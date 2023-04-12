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
        public Task<TResponse?> ReadAsync(TRequest request);
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
            return (T)Activator.CreateInstance(
                typeof(T),
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                Type.DefaultBinder, 
                new object[] { command }, 
                null, 
                null)!;
        }


    }

    public abstract class DatabaseReader : DatabaseAccessor, IDatabaseReader
    {
        protected DatabaseReader(NpgsqlCommand command): base(command)
        {
        }

    }

    public abstract class DatabaseReaderBase<TRequest, TResponse>: DatabaseReader
    {
        protected DatabaseReaderBase(NpgsqlCommand command) : base(command)
        {
        }

        protected abstract IEnumerable<ParameterValue> GetParameterValues(TRequest request);
        protected abstract TResponse Read(NpgsqlDataReader reader);

    }
    public abstract class IntDatabaseReader<TRequest> : MandatorySingleItemDatabaseReader<TRequest, int>, ISingleItemDatabaseReader<TRequest, int>
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
    public abstract class MandatorySingleItemDatabaseReader<TRequest, TResponse> : DatabaseReaderBase<TRequest, TResponse>, ISingleItemDatabaseReader<TRequest, TResponse>
    {
        protected MandatorySingleItemDatabaseReader(NpgsqlCommand command) : base(command)
        {
        }

        protected abstract string GetErrorMessage(TRequest request);

        public async Task<TResponse?> ReadAsync(TRequest request)
        {
            Set(GetParameterValues(request));
            await using var reader = await _command.ExecuteReaderAsync();
            if (!reader.HasRows)
                throw new Exception(GetErrorMessage(request)); ;
            await reader.ReadAsync();
            return Read(reader);
        }
    }

    public abstract class SingleItemDatabaseReader<TRequest, TResponse> : DatabaseReaderBase<TRequest, TResponse>, ISingleItemDatabaseReader<TRequest, TResponse>
        where TResponse : class
    {
        protected SingleItemDatabaseReader(NpgsqlCommand command) : base(command)
        {
        }
        public async Task<TResponse?> ReadAsync(TRequest request)
        {
            Set(GetParameterValues(request));
            await using var reader = await _command.ExecuteReaderAsync();
            if (!reader.HasRows)
                return null;
            await reader.ReadAsync();
            return Read(reader);
        }
    }
    public abstract class EnumerableDatabaseReader<TRequest, TResponse> : DatabaseReaderBase<TRequest, TResponse>, IEnumerableDatabaseReader<TRequest, TResponse>
    {
        protected EnumerableDatabaseReader(NpgsqlCommand command) : base(command)
        {
        }
        public async IAsyncEnumerable<TResponse> ReadAsync(TRequest request)
        {
            Set(GetParameterValues(request));
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
