using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace PoundPupLegacy.Common;
public interface IDatabaseInserter : IAsyncDisposable { }
public interface IDatabaseInserter<T>: IDatabaseInserter
{
    Task InsertAsync(T item);
}
public interface IDatabaseInserterFactory 
{ 
}
public interface IDatabaseInserterFactory<T>: IDatabaseInserterFactory
{
    Task<IDatabaseInserter<T>> CreateAsync(IDbConnection connection);
}

public abstract class DatabaseInserterFactory<T> : IDatabaseInserterFactory<T>
{

    public abstract Task<IDatabaseInserter<T>> CreateAsync(IDbConnection connection);

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

public abstract class DatabaseInserter<T> : DatabaseWriter, IDatabaseInserter<T>
{
    protected DatabaseInserter(NpgsqlCommand command): base(command)
    {
    }

    public abstract Task InsertAsync(T item);

}
