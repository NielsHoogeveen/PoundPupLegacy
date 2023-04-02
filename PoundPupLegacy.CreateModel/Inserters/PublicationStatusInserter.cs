namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PublicationStatusInserter : DatabaseInserter<PublicationStatus>, IDatabaseInserter<PublicationStatus>
{
    private const string ID = "id";
    private const string NAME = "name";
    public static async Task<DatabaseInserter<PublicationStatus>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "publication_status",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new PublicationStatusInserter(command);

    }

    internal PublicationStatusInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PublicationStatus nodeStatus)
    {
        if (nodeStatus.Id is null)
            throw new NullReferenceException();

        WriteValue(nodeStatus.Id, ID);
        WriteValue(nodeStatus.Name, NAME);
        await _command.ExecuteNonQueryAsync();
    }
}
