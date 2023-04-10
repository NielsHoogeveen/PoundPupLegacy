using Npgsql;
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

public abstract class BasicDatabaseInserterFactory<T, T2>: DatabaseInserterFactory<T>
    where T2 : BasicDatabaseInserter<T>
{
    public abstract string TableName { get; }

    public IEnumerable<DatabaseParameter> DatabaseParameters => GetDatabaseParameters();
    private List<DatabaseParameter> GetDatabaseParameters()
    {
        var t = GetType();
        var fields = t.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        return fields.Select(x => x.GetValue(null) as DatabaseParameter).Where(x => x is not null).Select(x => (DatabaseParameter)x!).ToList();
    }

    public sealed override async Task<IDatabaseInserter<T>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""
            INSERT INTO public."{TableName}"(
                {string.Join(',', DatabaseParameters.Select(x => x.Name))}
            ) 
            VALUES(
                {string.Join(',', DatabaseParameters.Select(x => $"@{x.Name}"))}
            )
            """;

        foreach (var parameter in DatabaseParameters) {
            command.AddParameter(parameter);
        }
        await command.PrepareAsync();
        return (IDatabaseInserter<T>)Activator.CreateInstance(typeof(T2), new object[] { command })!;
    }
}


public abstract class DatabaseInserterFactory<T> : IDatabaseInserterFactory<T>
{

    public abstract Task<IDatabaseInserter<T>> CreateAsync(IDbConnection connection);

    protected static async Task<NpgsqlCommand> CreateAutoGenerateIdentityInsertStatementAsync(NpgsqlConnection connection, string tableName, IEnumerable<DatabaseParameter> databaseParameters)
    {
        var sql = $"""
            INSERT INTO public."{tableName}"(
                {string.Join(',', databaseParameters.Select(x => x.Name))}
            ) 
            VALUES(
                {string.Join(',', databaseParameters.Select(x => $"@{x.Name}"))}
            );
            SELECT lastval();
            """;
        var sqlEmpty = $"""
            INSERT INTO public."{tableName}" DEFAULT VALUES;
            SELECT lastval();
            """;

        return await CreatePreparedStatementAsync(connection, databaseParameters, databaseParameters.Any() ? sql : sqlEmpty);
    }

    protected static async Task<NpgsqlCommand> CreateInsertStatementAsync(NpgsqlConnection connection, string tableName, IEnumerable<DatabaseParameter> databaseParameters)
    {
        var sql = $"""
            INSERT INTO public."{tableName}"(
                {string.Join(',', databaseParameters.Select(x => x.Name))}
            ) 
            VALUES(
                {string.Join(',', databaseParameters.Select(x => $"@{x.Name}"))}
            )
            """;
        return await CreatePreparedStatementAsync(connection, databaseParameters, sql);
    }
    protected static async Task<NpgsqlCommand> CreatePreparedStatementAsync(NpgsqlConnection connection, IEnumerable<DatabaseParameter> columnDefinitions, string sql)
    {

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        foreach (var column in columnDefinitions) {
            command.Parameters.Add(column.Name, column.ParameterType);
        }
        await command.PrepareAsync();
        return command;
    }
}
public abstract class BasicDatabaseInserter<T> : DatabaseInserter<T>
{
    public abstract IEnumerable<ParameterValue> GetParameterValues(T item);
    protected BasicDatabaseInserter(NpgsqlCommand command) : base(command)
    {
    }
    public sealed override async Task InsertAsync(T item)
    {
        Set(GetParameterValues(item));
        await _command.ExecuteNonQueryAsync();
    }
}

public abstract class DatabaseInserter<T> : DatabaseWriter, IDatabaseInserter<T>
{
    protected DatabaseInserter(NpgsqlCommand command): base(command)
    {
    }

    public abstract Task InsertAsync(T item);

}
