﻿namespace PoundPupLegacy.CreateModel.Inserters;

public interface IDatabaseInserter
{

}
public interface IDatabaseInserter<T> : IDatabaseInserter
{
    public abstract static Task<DatabaseInserter<T>> CreateAsync(NpgsqlConnection connection);
}

public abstract class DatabaseInserter : IDatabaseInserter
{

    protected struct ColumnDefinition
    {
        public required string Name { get; init; }
        public required NpgsqlDbType NpgsqlDbType { get; init; }
    }
    protected static async Task<NpgsqlCommand> CreateIdentityInsertStatementAsync(NpgsqlConnection connection, string tableName, IEnumerable<ColumnDefinition> columnDefinitions)
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
        var sqlEmpty = $"""
            INSERT INTO public."{tableName}" DEFAULT VALUES;
            SELECT lastval();
            """;

        return await CreatePreparedStatementAsync(connection, columnDefinitions, columnDefinitions.Any() ? sql : sqlEmpty);
    }

    protected static async Task<NpgsqlCommand> CreateInsertStatementAsync(NpgsqlConnection connection, string tableName, IEnumerable<ColumnDefinition> columnDefinitions)
    {
        var sql = $"""
            INSERT INTO public."{tableName}"(
                {string.Join(',', columnDefinitions.Select(x => x.Name))}
            ) 
            VALUES(
                {string.Join(',', columnDefinitions.Select(x => $"@{x.Name}"))}
            )
            """;
        return await CreatePreparedStatementAsync(connection, columnDefinitions, sql);
    }
    protected static async Task<NpgsqlCommand> CreatePreparedStatementAsync(NpgsqlConnection connection, IEnumerable<ColumnDefinition> columnDefinitions, string sql)
    {

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        foreach (var column in columnDefinitions) {
            command.Parameters.Add(column.Name, column.NpgsqlDbType);
        }
        await command.PrepareAsync();
        return command;
    }


}
public abstract class DatabaseInserter<T> : DatabaseInserter, IAsyncDisposable
{
    protected readonly NpgsqlCommand _command;

    protected DatabaseInserter(NpgsqlCommand command)
    {
        _command = command;
    }

    public abstract Task InsertAsync(T item);
    protected void WriteDateTimeRange(DateTimeRange? dateTimeRange, string parameterDateRange)
    {
        WriteDateTimeRange(dateTimeRange, parameterDateRange, _command);
    }
    protected void WriteDateTimeRange(DateTimeRange? dateTimeRange, string parameterDateRange, NpgsqlCommand command)
    {
        if (dateTimeRange is null) {
            command.Parameters[parameterDateRange].Value = DBNull.Value;
        }
        else {
            if (dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue) {
                if (dateTimeRange.Start.Equals(dateTimeRange.End.Value)) {
                    throw new ArgumentException("A date range should have a start that is below the end");

                }
                else {
                    command.Parameters[parameterDateRange].Value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd")}, {dateTimeRange.End.Value.ToString("yyyy-MM-dd")})";
                }

            }
            else if (!dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue) {
                command.Parameters[parameterDateRange].Value = $"(, {dateTimeRange.End.Value.ToString("yyyy-MM-dd")})";

            }
            else if (dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue) {
                command.Parameters[parameterDateRange].Value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd")},)";

            }
            else if (!dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue) {
                command.Parameters[parameterDateRange].Value = $"(,)";

            }
        }
    }

    protected void WriteDateTimeRange(DateTimeRange? dateTimeRange, string parameterDate, string parameterDateRange)
    {
        WriteDateTimeRange(dateTimeRange, parameterDate, parameterDateRange, _command);
    }
    protected void WriteDateTimeRange(DateTimeRange? dateTimeRange, string parameterDate, string parameterDateRange, NpgsqlCommand command)
    {
        if (dateTimeRange is null) {
            command.Parameters[parameterDateRange].Value = DBNull.Value;
            command.Parameters[parameterDate].Value = DBNull.Value;
        }
        else {
            if (dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue) {
                if (dateTimeRange.Start.Equals(dateTimeRange.End.Value)) {
                    command.Parameters[parameterDateRange].Value = DBNull.Value;
                    command.Parameters[parameterDate].Value = dateTimeRange.Start;

                }
                else {
                    command.Parameters[parameterDateRange].Value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd")}, {dateTimeRange.End.Value.ToString("yyyy-MM-dd")})";
                    command.Parameters[parameterDate].Value = DBNull.Value;
                }

            }
            else if (!dateTimeRange.Start.HasValue && dateTimeRange.End.HasValue) {
                command.Parameters[parameterDateRange].Value = $"(, {dateTimeRange.End.Value.ToString("yyyy-MM-dd")})";
                command.Parameters[parameterDate].Value = DBNull.Value;

            }
            else if (dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue) {
                command.Parameters[parameterDateRange].Value = $"[{dateTimeRange.Start.Value.ToString("yyyy-MM-dd")},)";
                command.Parameters[parameterDate].Value = DBNull.Value;

            }
            else if (!dateTimeRange.Start.HasValue && !dateTimeRange.End.HasValue) {
                command.Parameters[parameterDateRange].Value = $"(,)";
                command.Parameters[parameterDate].Value = DBNull.Value;

            }
        }
    }

    protected void WriteNullableValue<T2>(T2? value, string parameter)
    {
        WriteNullableValue(value, parameter, _command);
    }
    protected void WriteNullableValue<T2>(T2? value, string parameter, NpgsqlCommand command)
    {
        if (value is not null) {
            command.Parameters[parameter].Value = value;
        }
        else {
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
    public async virtual ValueTask DisposeAsync()
    {
        await _command.DisposeAsync();
    }

}