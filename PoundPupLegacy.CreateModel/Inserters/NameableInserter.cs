namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class NameableInserter : DatabaseInserter<Nameable>, IDatabaseInserter<Nameable>
{
    private const string ID = "id";
    private const string DESCRIPTION = "description";
    private const string FILE_ID_TILE_IMAGE = "file_id_tile_image";
    public static async Task<DatabaseInserter<Nameable>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "nameable",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = FILE_ID_TILE_IMAGE,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new NameableInserter(command);

    }

    internal NameableInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Nameable nameable)
    {
        if (nameable.Id is null)
            throw new NullReferenceException();

        WriteValue(nameable.Id, ID);
        WriteValue(nameable.Description, DESCRIPTION);
        WriteNullableValue(nameable.FileIdTileImage, FILE_ID_TILE_IMAGE);
        await _command.ExecuteNonQueryAsync();
    }
}
