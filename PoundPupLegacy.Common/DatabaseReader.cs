using Npgsql;
using System.Data;

namespace PoundPupLegacy.Common
{
    public interface IDatabaseReaderFactory : IDatabaseAccessorFactory
    {
        string Sql { get; }
    }
    public interface IDatabaseReaderFactory<TRequest, TResponse> : IDatabaseReaderFactory
        where TRequest : IRequest
    {

    }

    public interface IDatabaseReader : IDatabaseAccessor
    {

    }
    public interface IDatabaseReader<TRequest, TResponse> : IDatabaseReader
        where TRequest : IRequest
    {

    }

    public interface ISingleItemDatabaseReader<TRequest, TResponse> : IDatabaseReader<TRequest, TResponse>
        where TRequest : IRequest
    {
        public Task<TResponse?> ReadAsync(TRequest request);
    }
    public interface IMandatorySingleItemDatabaseReader<TRequest, TResponse> : IDatabaseReader<TRequest, TResponse>
    where TRequest : IRequest
    {
        public Task<TResponse> ReadAsync(TRequest request);
    }
    public interface IDoesRecordExistDatabaseReader<TRequest> : IDatabaseReader<TRequest, bool>
        where TRequest : IRequest
    {
        public Task<bool> ReadAsync(TRequest request);
    }


    public interface IEnumerableDatabaseReader<TRequest, TResponse> : IDatabaseReader<TRequest, TResponse>
        where TRequest : IRequest
    {
        public IAsyncEnumerable<TResponse> ReadAsync(TRequest request);
    }

    public interface ISingleItemDatabaseReaderFactory<TRequest, TResponse> : IDatabaseReaderFactory<TRequest, TResponse>
        where TRequest : IRequest
    {
        public Task<ISingleItemDatabaseReader<TRequest, TResponse>> CreateAsync(IDbConnection connection);
    }
    public interface IDoesRecordExistDatabaseReaderFactory<TRequest> : IDatabaseReaderFactory<TRequest, bool>
        where TRequest : IRequest
    {
        public Task<IDoesRecordExistDatabaseReader<TRequest>> CreateAsync(IDbConnection connection);
    }


    public abstract class SingleItemDatabaseReaderFactory<TRequest, TResponse> : DatabaseReaderFactory<TRequest, TResponse>, ISingleItemDatabaseReaderFactory<TRequest, TResponse>
        where TResponse : class
        where TRequest : IRequest
    {
        protected abstract TResponse? Read(NpgsqlDataReader reader);
        public async Task<ISingleItemDatabaseReader<TRequest, TResponse>> CreateAsync(IDbConnection connection)
        {
            return new SingleItemDatabaseReader<TRequest, TResponse>(await GetCommand(connection), GetParameterValues, Read);
        }
    }
    public interface IMandatorySingleItemDatabaseReaderFactory<TRequest, TResponse> : IDatabaseReaderFactory<TRequest, TResponse>
        where TRequest : IRequest
    {
        public Task<IMandatorySingleItemDatabaseReader<TRequest, TResponse>> CreateAsync(IDbConnection connection);
    }
    public abstract class IntDatabaseReaderFactory<TRequest> : DatabaseReaderFactory<TRequest, int>, IMandatorySingleItemDatabaseReaderFactory<TRequest, int>
        where TRequest : IRequest
    {
        protected abstract string GetErrorMessage(TRequest request);

        protected abstract IntValueReader IntValueReader { get; }

        public async Task<IMandatorySingleItemDatabaseReader<TRequest, int>> CreateAsync(IDbConnection connection)
        {
            return new IntDatabaseReader<TRequest>(await GetCommand(connection), IntValueReader, GetParameterValues, GetErrorMessage);
        }
    }
    public abstract class DoesRecordExistDatabaseReaderFactory<TRequest> : DatabaseReaderFactory<TRequest, bool>, IDoesRecordExistDatabaseReaderFactory<TRequest>
    where TRequest : IRequest
    {
        public async Task<IDoesRecordExistDatabaseReader<TRequest>> CreateAsync(IDbConnection connection)
        {
            return new DoesRecordExistDatabaseReader<TRequest>(await GetCommand(connection), GetParameterValues);
        }
    }

    public abstract class MandatorySingleItemDatabaseReaderFactory<TRequest, TResponse> : DatabaseReaderFactory<TRequest, TResponse>, IMandatorySingleItemDatabaseReaderFactory<TRequest, TResponse>
        where TRequest : IRequest
    {
        protected abstract TResponse Read(NpgsqlDataReader reader);

        protected abstract string GetErrorMessage(TRequest request);

        public async Task<IMandatorySingleItemDatabaseReader<TRequest, TResponse>> CreateAsync(IDbConnection connection)
        {
            return new MandatorySingleItemDatabaseReader<TRequest, TResponse>(await GetCommand(connection), GetParameterValues, Read, GetErrorMessage);
        }
    }
    public interface IEnumerableDatabaseReaderFactory<TRequest, TResponse> : IDatabaseReaderFactory<TRequest, TResponse>
    where TRequest : IRequest
    {
        public Task<IEnumerableDatabaseReader<TRequest, TResponse>> CreateAsync(IDbConnection connection);
    }

    public abstract class EnumerableDatabaseReaderFactory<TRequest, TResponse> : DatabaseReaderFactory<TRequest, TResponse>, IEnumerableDatabaseReaderFactory<TRequest, TResponse>
        where TRequest : IRequest
    {

        protected abstract TResponse Read(NpgsqlDataReader reader);
        public async Task<IEnumerableDatabaseReader<TRequest, TResponse>> CreateAsync(IDbConnection connection)
        {
            return new EnumerableDatabaseReader<TRequest, TResponse>(await GetCommand(connection), GetParameterValues, Read);
        }

    }

    public abstract class DatabaseReaderFactory<TRequest, TResponse> : DatabaseAccessorFactory, IDatabaseReaderFactory<TRequest, TResponse>
        where TRequest : IRequest
    {
        public abstract string Sql { get; }

        protected abstract IEnumerable<ParameterValue> GetParameterValues(TRequest request);

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


    public class IntDatabaseReader<TRequest> : MandatorySingleItemDatabaseReader<TRequest, int>
        where TRequest : IRequest
    {
        public IntDatabaseReader(
            NpgsqlCommand command,
            IntValueReader IntValueReader,
            Func<TRequest, IEnumerable<ParameterValue>> parameterMapper,
            Func<TRequest, string> errorMessageFunction) :
            base(command, parameterMapper, (reader) => IntValueReader.GetValue(reader), errorMessageFunction)
        {
        }
    }

    public class DoesRecordExistDatabaseReader<TRequest> : DatabaseAccessor<TRequest>, IDoesRecordExistDatabaseReader<TRequest>
        where TRequest : IRequest
    {
        private readonly Func<TRequest, IEnumerable<ParameterValue>> _parameterMapper;

        public DoesRecordExistDatabaseReader(NpgsqlCommand command, Func<TRequest, IEnumerable<ParameterValue>> parameterMapper) : base(command)
        {
            _parameterMapper = parameterMapper;
        }
        protected override IEnumerable<ParameterValue> GetParameterValues(TRequest request)
        {
            return _parameterMapper(request);
        }

        public async Task<bool> ReadAsync(TRequest request)
        {
            foreach (var parameter in GetParameterValues(request)) {
                parameter.Set(_command);
            }
            await using var reader = await _command.ExecuteReaderAsync();
            return reader.HasRows;
        }
    }


    public class MandatorySingleItemDatabaseReader<TRequest, TResponse> : DatabaseAccessor<TRequest>, IMandatorySingleItemDatabaseReader<TRequest, TResponse>
        where TRequest : IRequest
    {
        private readonly Func<NpgsqlDataReader, TResponse> _readerFunction;
        private readonly Func<TRequest, IEnumerable<ParameterValue>> _parameterMapper;
        private readonly Func<TRequest, string> _errorMessageFunction;

        public MandatorySingleItemDatabaseReader(NpgsqlCommand command, Func<TRequest, IEnumerable<ParameterValue>> parameterMapper, Func<NpgsqlDataReader, TResponse> readerFunction, Func<TRequest, string> errorMessageFunction) : base(command)
        {
            _parameterMapper = parameterMapper;
            _readerFunction = readerFunction;
            _errorMessageFunction = errorMessageFunction;
        }
        protected override IEnumerable<ParameterValue> GetParameterValues(TRequest request)
        {
            return _parameterMapper(request);
        }

        public async Task<TResponse> ReadAsync(TRequest request)
        {
            foreach (var parameter in GetParameterValues(request)) {
                parameter.Set(_command);
            }
            await using var reader = await _command.ExecuteReaderAsync();
            if (!reader.HasRows)
                throw new Exception(_errorMessageFunction(request)); ;
            await reader.ReadAsync();
            return _readerFunction(reader);
        }
    }

    public class SingleItemDatabaseReader<TRequest, TResponse> : DatabaseAccessor<TRequest>, ISingleItemDatabaseReader<TRequest, TResponse>
        where TResponse : class
        where TRequest : IRequest
    {
        private readonly Func<NpgsqlDataReader, TResponse?> _readerFunction;
        private readonly Func<TRequest, IEnumerable<ParameterValue>> _parameterMapper;
        protected override IEnumerable<ParameterValue> GetParameterValues(TRequest request)
        {
            return _parameterMapper(request);
        }

        public SingleItemDatabaseReader(NpgsqlCommand command, Func<TRequest, IEnumerable<ParameterValue>> parameterMapper, Func<NpgsqlDataReader, TResponse?> readerFunction) : base(command)
        {
            _readerFunction = readerFunction;
            _parameterMapper = parameterMapper;
        }

        public async Task<TResponse?> ReadAsync(TRequest request)
        {
            foreach (var parameter in GetParameterValues(request)) {
                parameter.Set(_command);
            }
            await using var reader = await _command.ExecuteReaderAsync();
            if (!reader.HasRows)
                return null;
            await reader.ReadAsync();

            return _readerFunction(reader);
        }
    }
    public class EnumerableDatabaseReader<TRequest, TResponse> : DatabaseAccessor<TRequest>, IEnumerableDatabaseReader<TRequest, TResponse>
        where TRequest : IRequest
    {
        private readonly Func<NpgsqlDataReader, TResponse> _readerFunction;
        private readonly Func<TRequest, IEnumerable<ParameterValue>> _parameterMapper;

        public EnumerableDatabaseReader(NpgsqlCommand command, Func<TRequest, IEnumerable<ParameterValue>> parameterMapper, Func<NpgsqlDataReader, TResponse> readerFunction) : base(command)
        {
            _readerFunction = readerFunction;
            _parameterMapper = parameterMapper;
        }
        public async IAsyncEnumerable<TResponse> ReadAsync(TRequest request)
        {
            foreach (var parameter in GetParameterValues(request)) {
                parameter.Set(_command);
            }
            await using var reader = await _command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) {
                yield return _readerFunction(reader);
            }
        }

        protected override IEnumerable<ParameterValue> GetParameterValues(TRequest request)
        {
            return _parameterMapper(request);
        }
    }
    public abstract record class ValueReader<T>
    {
        public required string Name { get; init; }

        public abstract T GetValue(NpgsqlDataReader reader);
    }
    public sealed record class FieldValueReader<T> : ValueReader<T>
    {
        public override T GetValue(NpgsqlDataReader reader)
        {
            return reader.GetFieldValue<T>(reader.GetOrdinal(Name));
        }
    }

    public sealed record class IntValueReader : ValueReader<int>
    {
        public override int GetValue(NpgsqlDataReader reader)
        {
            return reader.GetInt32(reader.GetOrdinal(Name));
        }
    }
    public sealed record class NullableIntValueReader : ValueReader<int?>
    {
        public override int? GetValue(NpgsqlDataReader reader)
        {
            var ordinal = reader.GetOrdinal(Name);
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetInt32(ordinal);
        }
    }
    public sealed record class LongValueReader : ValueReader<long>
    {
        public override long GetValue(NpgsqlDataReader reader)
        {
            return reader.GetInt64(reader.GetOrdinal(Name));
        }
    }
    public sealed record class NullableLongValueReader : ValueReader<long?>
    {
        public override long? GetValue(NpgsqlDataReader reader)
        {
            var ordinal = reader.GetOrdinal(Name);
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetInt64(ordinal);
        }
    }
    public sealed record class StringValueReader : ValueReader<string>
    {
        public override string GetValue(NpgsqlDataReader reader)
        {
            return reader.GetString(reader.GetOrdinal(Name));
        }
    }
    public sealed record class NullableStringValueReader : ValueReader<string?>
    {
        public override string? GetValue(NpgsqlDataReader reader)
        {
            var ordinal = reader.GetOrdinal(Name);
            if (reader.IsDBNull(ordinal))
                return null;
            else
                return reader.GetString(ordinal);
        }
    }

    public sealed record class DateTimeValueReader : ValueReader<DateTime>
    {
        public override DateTime GetValue(NpgsqlDataReader reader)
        {
            return reader.GetDateTime(reader.GetOrdinal(Name));
        }
    }
    public sealed record class NullableDateTimeValueReader : ValueReader<DateTime?>
    {
        public override DateTime? GetValue(NpgsqlDataReader reader)
        {
            var ordinal = (int)reader.GetOrdinal(Name);
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetDateTime(ordinal);
        }
    }
    public sealed record class BooleanValueReader : ValueReader<bool>
    {
        public override bool GetValue(NpgsqlDataReader reader)
        {
            return reader.GetBoolean(reader.GetOrdinal(Name));
        }
    }
    public sealed record class NullableBooleanValueReader : ValueReader<bool?>
    {
        public override bool? GetValue(NpgsqlDataReader reader)
        {
            var ordinal = reader.GetOrdinal(Name);
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetBoolean(ordinal);
        }
    }

    public sealed record class IntListValueReader : ValueReader<List<int>>
    {
        public override List<int> GetValue(NpgsqlDataReader reader)
        {
            return reader.GetFieldValue<int[]>(reader.GetOrdinal(Name)).ToList();
        }
    }
}
