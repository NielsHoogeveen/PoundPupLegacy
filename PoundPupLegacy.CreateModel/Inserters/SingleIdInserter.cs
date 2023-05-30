namespace PoundPupLegacy.CreateModel.Inserters;

internal static class SingleIdInserterFactory
{
    internal static NullCheckingIntegerDatabaseParameter Id = new() { Name = "id" };
}
internal abstract class SingleIdInserterFactory<T> : IDatabaseInserterFactory<T>
    where T : PossiblyIdentifiable
{
    protected abstract string TableName { get; }

    protected abstract bool AutoGenerateIdentity { get; }

    public IEnumerable<DatabaseParameter> DatabaseParameters => new List<DatabaseParameter> { SingleIdInserterFactory.Id };

    public async Task<IDatabaseInserter<T>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var databaseParameters = new List<DatabaseParameter>();
        if (!AutoGenerateIdentity) {
            databaseParameters.Add(SingleIdInserterFactory.Id);
        }
        var command = !AutoGenerateIdentity ?
            await CreateInsertStatementAsync(
            postgresConnection,
            TableName,
            databaseParameters
        ) : await CreateAutoGenerateIdentityInsertStatementAsync(
            postgresConnection,
            TableName,
            databaseParameters
        );
        return new SingleIdInserter<T>(command, AutoGenerateIdentity, TableName);

    }
    private static async Task<NpgsqlCommand> CreateAutoGenerateIdentityInsertStatementAsync(NpgsqlConnection connection, string tableName, IEnumerable<DatabaseParameter> databaseParameters)
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

    private static async Task<NpgsqlCommand> CreateInsertStatementAsync(NpgsqlConnection connection, string tableName, IEnumerable<DatabaseParameter> databaseParameters)
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
internal sealed class SingleIdInserter<T> : DatabaseAccessor<T>, IDatabaseInserter<T> 
    where T : PossiblyIdentifiable
{

    private readonly bool _autoGenerateIdentity;
    private readonly string _tableName;
    internal SingleIdInserter(NpgsqlCommand command, bool autoGenerateIdentity, string tableName) : base(command)
    {
        _autoGenerateIdentity = autoGenerateIdentity;
        _tableName = tableName;
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(T request)
    {
        return new ParameterValue[] { ParameterValue.Create(SingleIdInserterFactory.Id, request.IdentificationForCreate.Id) };
    }

    public async Task InsertAsync(T request)
    {
        if (!_autoGenerateIdentity) {
            foreach (var parameterValue in GetParameterValues(request)) {
                parameterValue.Set(_command);
            }
            await _command.ExecuteNonQueryAsync();
        }
        else {
            if (request.IdentificationForCreate.Id is not null)
                throw new Exception($"Id should be null when adding a {_tableName}");
            request.IdentificationForCreate.Id = await _command.ExecuteScalarAsync() switch {
                long i => (int)i,
                _ => throw new Exception($"No id has been assigned when adding a {_tableName}"),
            };
        }
    }
}