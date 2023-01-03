using System.Data;

namespace PoundPupLegacy.Db.Writers;

public interface IDatabaseWriter
{

}
public interface IDatabaseWriter<T> : IDatabaseWriter
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

    public virtual void Dispose()
    {
        _command.Dispose();
    }

    internal abstract void Write(T item);

    protected void WriteDateTimeRange(DateTimeRange? dateTimeRange, string parameterDate, string parameterDateRange)
    {
        if (dateTimeRange is null)
        {
            _command.Parameters[parameterDateRange].Value = DBNull.Value;
            _command.Parameters[parameterDate].Value = DBNull.Value;
        }
        else
        {
            if (dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue)
            {
                if (dateTimeRange.Start.Equals(dateTimeRange.End.Value))
                {
                    _command.Parameters[parameterDateRange].Value = DBNull.Value;
                    _command.Parameters[parameterDate].Value = dateTimeRange.Start;

                }
                else
                {
                    _command.Parameters[parameterDateRange].Value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd")}, {dateTimeRange.End.Value.ToString("yyyy-MM-dd")})";
                    _command.Parameters[parameterDate].Value = DBNull.Value;
                }

            }
            else if (!dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue)
            {
                _command.Parameters[parameterDateRange].Value = $"(, {dateTimeRange.End.Value.ToString("yyyy-MM-dd")})";
                _command.Parameters[parameterDate].Value = DBNull.Value;

            }
            else if (dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue)
            {
                _command.Parameters[parameterDateRange].Value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd")},)";
                _command.Parameters[parameterDate].Value = DBNull.Value;

            }
            else if (!dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue)
            {
                _command.Parameters[parameterDateRange].Value = $"(,)";
                _command.Parameters[parameterDate].Value = DBNull.Value;

            }
        }
    }
    protected void WriteNullableValue<T2>(T2? value, string parameter)
    {
        WriteNullableValue(value, parameter, _command);
    }
    protected void WriteNullableValue<T2>(T2? value, string parameter, NpgsqlCommand command)
    {
        if (value is not null)
        {
            command.Parameters[parameter].Value = value;
        }
        else
        {
            command.Parameters[parameter].Value = DBNull.Value;
        }
    }
    protected void WriteValue<T2>(T2 value, string parameter)
    {
        WriteValue(value, parameter, _command);
    }

    protected void WriteValue<T2>(T2 value, string parameter, NpgsqlCommand command)
    {
        command.Parameters[parameter].Value = value;
    }

    protected struct ColumnDefinition
    {
        public required string Name { get; init; }
        public required NpgsqlDbType NpgsqlDbType { get; init; }
    }

    protected static NpgsqlCommand CreateIdentityInsertStatement(NpgsqlConnection connection, string tableName, IEnumerable<ColumnDefinition> columnDefinitions)
    {
        var sql = $"""
            INSERT INTO public."{tableName}"(
                {string.Join(',', columnDefinitions.Select(x => x.Name))}
            ) 
            VALUES(
                {string.Join(',', columnDefinitions.Select(x => $"@{x.Name}"))}
            );
            SELECT lastval();
            """;
        return CreatePreparedStatement(connection, columnDefinitions, sql);
    }

    protected static NpgsqlCommand CreateInsertStatement(NpgsqlConnection connection, string tableName, IEnumerable<ColumnDefinition> columnDefinitions)
    {
        var sql = $"""
            INSERT INTO public."{tableName}"(
                {string.Join(',', columnDefinitions.Select(x => x.Name))}
            ) 
            VALUES(
                {string.Join(',', columnDefinitions.Select(x => $"@{x.Name}"))}
            )
            """;
        return CreatePreparedStatement(connection, columnDefinitions, sql);
    }
    protected static NpgsqlCommand CreatePreparedStatement(NpgsqlConnection connection, IEnumerable<ColumnDefinition> columnDefinitions, string sql)
    {

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        foreach (var column in columnDefinitions)
        {
            command.Parameters.Add(column.Name, column.NpgsqlDbType);
        }
        command.Prepare();
        return command;
    }

}
