namespace PoundPupLegacy.CreateModel.Inserters;

internal abstract class SingleIdInserterFactory<T> : DatabaseInserterFactory<T>
    where T : Identifiable
{
    internal const string ID = "id";

    protected abstract string TableName { get; }

    protected abstract bool AutoGenerateIdentity { get; } 

    public override async Task<IDatabaseInserter<T>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var columnDefinitions = new List<ColumnDefinition>();
        if (!AutoGenerateIdentity) {
            columnDefinitions.Add(new ColumnDefinition {
                Name = ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            });
        }
        var command = !AutoGenerateIdentity ?
            await CreateInsertStatementAsync(
            postgresConnection,
            TableName,
            columnDefinitions
        ) : await CreateIdentityInsertStatementAsync(
            postgresConnection,
            TableName,
            columnDefinitions
        );
        return new SingleIdInserter<T>(command, AutoGenerateIdentity, TableName);

    }


}
internal sealed class SingleIdInserter<T> : DatabaseInserter<T> where T : Identifiable
{
    internal const string ID = "id";

    private readonly bool _autoGenerateIdentity;
    private readonly string _tableName;
    internal SingleIdInserter(NpgsqlCommand command, bool autoGenerateIdentity, string tableName) : base(command)
    {
        _autoGenerateIdentity = autoGenerateIdentity;
        _tableName = tableName;
    }

    public override async Task InsertAsync(T identifiable)
    {
        if (!_autoGenerateIdentity) {
            if (identifiable.Id is null)
                throw new NullReferenceException($"Id for {_tableName} should not be null");
            SetParameter(identifiable.Id, ID);
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