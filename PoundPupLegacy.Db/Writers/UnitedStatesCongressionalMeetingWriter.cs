namespace PoundPupLegacy.Db.Writers;

internal sealed class UnitedStatesCongressionalMeetingWriter : DatabaseWriter<UnitedStatesCongressionalMeeting>, IDatabaseWriter<UnitedStatesCongressionalMeeting>
{

    private const string ID = "id";
    private const string DATE_RANGE = "date_range";
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
        await _command.ExecuteNonQueryAsync();
    }

}
