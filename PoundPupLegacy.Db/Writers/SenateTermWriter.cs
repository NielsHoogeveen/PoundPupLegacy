namespace PoundPupLegacy.Db.Writers;

internal sealed class SenateTermWriter : DatabaseWriter<SenateTerm>, IDatabaseWriter<SenateTerm>
{
    private const string ID = "id";
    private const string SENATOR_ID = "senator_id";
    private const string SUBDIVISION_ID = "subdivision_id";
    private const string DATE_RANGE = "date_range";
    public static async Task<DatabaseWriter<SenateTerm>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "senate_term",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SENATOR_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SUBDIVISION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DATE_RANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
            }
        );
        return new SenateTermWriter(command);

    }

    internal SenateTermWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(SenateTerm senateTerm)
    {
        if (senateTerm.Id is null)
            throw new NullReferenceException();
        if (senateTerm.SenatorId is null)
            throw new NullReferenceException();
        WriteValue(senateTerm.Id, ID);
        WriteValue(senateTerm.SenatorId, SENATOR_ID);
        WriteValue(senateTerm.SubdivisionId, SUBDIVISION_ID);
        WriteDateTimeRange(senateTerm.DateTimeRange, DATE_RANGE);
        await _command.ExecuteNonQueryAsync();
    }
}
