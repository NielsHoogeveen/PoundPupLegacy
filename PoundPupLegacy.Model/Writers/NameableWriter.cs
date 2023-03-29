namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class NameableWriter : DatabaseWriter<Nameable>, IDatabaseWriter<Nameable>
{
    private const string ID = "id";
    private const string DESCRIPTION = "description";
    private const string FILE_ID_TILE_IMAGE = "file_id_tile_image";
    public static async Task<DatabaseWriter<Nameable>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
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
        return new NameableWriter(command);

    }

    internal NameableWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Nameable nameable)
    {
        if (nameable.Id is null)
            throw new NullReferenceException();

        WriteValue(nameable.Id, ID);
        WriteValue(nameable.Description, DESCRIPTION);
        WriteNullableValue(nameable.FileIdTileImage, FILE_ID_TILE_IMAGE);
        await _command.ExecuteNonQueryAsync();
    }
}
