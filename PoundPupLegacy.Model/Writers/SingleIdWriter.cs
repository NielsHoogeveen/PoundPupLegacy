namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class SingleIdWriter : DatabaseWriter
{
    internal const string ID = "id";
    internal static async Task<DatabaseWriter<T>> CreateSingleIdWriterAsync<T>(string tableName, NpgsqlConnection connection, bool insertIdentity = true)
        where T : Identifiable
    {
        var columnDefinitions = new List<ColumnDefinition>();
        if (insertIdentity) {
            columnDefinitions.Add(new ColumnDefinition {
                Name = ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            });
        }
        var command = insertIdentity ?
            await CreateInsertStatementAsync(
            connection,
            tableName,
            columnDefinitions
        ) : await CreateIdentityInsertStatementAsync(
            connection,
            tableName,
            columnDefinitions
        );
        return new SingleIdWriter<T>(command, insertIdentity, tableName);

    }


}
internal sealed class SingleIdWriter<T> : DatabaseWriter<T> where T : Identifiable
{
    internal const string ID = "id";

    private readonly bool _identityInsert;
    private readonly string _tableName;
    internal SingleIdWriter(NpgsqlCommand command, bool identityInsert, string tableName) : base(command)
    {
        _identityInsert = identityInsert;
        _tableName = tableName;
    }

    internal override async Task WriteAsync(T identifiable)
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