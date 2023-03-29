namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BasicSecondLevelSubdivisionInserter : DatabaseInserter<BasicSecondLevelSubdivision>, IDatabaseInserter<BasicSecondLevelSubdivision>
{
    private const string ID = "id";
    private const string INTERMEDIATE_LEVEL_SUBDIVISION_ID = "intermediate_level_subdivision_id";

    public static async Task<DatabaseInserter<BasicSecondLevelSubdivision>> CreateAsync(NpgsqlConnection connection)
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
        return new BasicSecondLevelSubdivisionInserter(command);
    }
    private BasicSecondLevelSubdivisionInserter(NpgsqlCommand command) : base(command)
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
