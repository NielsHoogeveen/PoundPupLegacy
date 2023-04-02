namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SingleIdInserter : DatabaseInserter
{
    internal const string ID = "id";
    internal static async Task<DatabaseInserter<T>> CreateSingleIdWriterAsync<T>(string tableName, IDbConnection connection, bool insertIdentity = true)
        where T : Identifiable
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var columnDefinitions = new List<ColumnDefinition>();
        if (insertIdentity) {
            columnDefinitions.Add(new ColumnDefinition {
                Name = ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            });
        }
        var command = insertIdentity ?
            await CreateInsertStatementAsync(
            postgresConnection,
            tableName,
            columnDefinitions
        ) : await CreateIdentityInsertStatementAsync(
            postgresConnection,
            tableName,
            columnDefinitions
        );
        return new SingleIdWriter<T>(command, insertIdentity, tableName);

    }


}
internal sealed class SingleIdWriter<T> : DatabaseInserter<T> where T : Identifiable
{
    internal const string ID = "id";

    private readonly bool _identityInsert;
    private readonly string _tableName;
    internal SingleIdWriter(NpgsqlCommand command, bool identityInsert, string tableName) : base(command)
    {
        _identityInsert = identityInsert;
        _tableName = tableName;
    }

    public override async Task InsertAsync(T identifiable)
    {
        if (_identityInsert) {
            if (identifiable.Id is null)
                throw new NullReferenceException($"Id for {_tableName} should not be null");
            WriteValue(identifiable.Id, ID);
            await _command.ExecuteNonQueryAsync();
        }
        else {
            if (identifiable.Id is not null)
                throw new Exception($"Id for {_tableName} should be set to null");
            identifiable.Id = await _command.ExecuteScalarAsync() switch {
                long i => (int)i,
                _ => throw new Exception($"No id has been assigned when adding a {_tableName}"),
            };
        }
    }
}