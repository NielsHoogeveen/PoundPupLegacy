namespace PoundPupLegacy.Db.Writers;

internal sealed class BasicSecondLevelSubdivisionWriter : DatabaseWriter<BasicSecondLevelSubdivision>, IDatabaseWriter<BasicSecondLevelSubdivision>
{
    private const string ID = "id";
    private const string INTERMEDIATE_LEVEL_SUBDIVISION_ID = "intermediate_level_subdivision_id";

    public static async Task<DatabaseWriter<BasicSecondLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "basic_second_level_subdivision",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = INTERMEDIATE_LEVEL_SUBDIVISION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new BasicSecondLevelSubdivisionWriter(command);
    }
    private BasicSecondLevelSubdivisionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(BasicSecondLevelSubdivision country)
    {
        if (country.Id is null)
            throw new NullReferenceException();
        WriteValue(country.Id, ID);
        WriteValue(country.IntermediateLevelSubdivisionId, INTERMEDIATE_LEVEL_SUBDIVISION_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
