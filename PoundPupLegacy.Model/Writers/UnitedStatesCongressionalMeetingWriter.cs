namespace PoundPupLegacy.CreateModel.Writers;
internal sealed class UnitedStatesCongressionalMeetingWriter : DatabaseWriter<UnitedStatesCongressionalMeeting>, IDatabaseWriter<UnitedStatesCongressionalMeeting>
{

    private const string ID = "id";
    private const string DATE_RANGE = "date_range";
    private const string NUMBER = "number";
    public static async Task<DatabaseWriter<UnitedStatesCongressionalMeeting>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "united_states_congressional_meeting",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DATE_RANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
                new ColumnDefinition{
                    Name = NUMBER,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new UnitedStatesCongressionalMeetingWriter(command);
    }

    internal UnitedStatesCongressionalMeetingWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(UnitedStatesCongressionalMeeting unitedStatesCongressionalMeeting)
    {
        if (unitedStatesCongressionalMeeting.Id is null)
            throw new NullReferenceException();
        WriteValue(unitedStatesCongressionalMeeting.Id, ID);
        WriteDateTimeRange(unitedStatesCongressionalMeeting.DateRange, DATE_RANGE);
        WriteValue(unitedStatesCongressionalMeeting.Number, NUMBER);
        await _command.ExecuteNonQueryAsync();
    }

}
