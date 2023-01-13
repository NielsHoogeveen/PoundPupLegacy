namespace PoundPupLegacy.Db.Writers;

internal sealed class PoliticalEntityWriter : DatabaseWriter<PoliticalEntity>, IDatabaseWriter<PoliticalEntity>
{
    private const string ID = "id";
    private const string FILE_ID_FLAG = "file_id_flag";
    public static async Task<DatabaseWriter<PoliticalEntity>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "political_entity",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = FILE_ID_FLAG,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new PoliticalEntityWriter(command);
    }

    internal PoliticalEntityWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(PoliticalEntity entity)
    {
        if (entity.Id is null)
            throw new NullReferenceException();

        WriteValue(entity.Id, ID);
        WriteNullableValue(entity.FileIdFlag, FILE_ID_FLAG);
        await _command.ExecuteNonQueryAsync();
    }
}
