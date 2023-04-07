namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class UnitedStatesCongressionalMeetingInserterFactory : DatabaseInserterFactory<UnitedStatesCongressionalMeeting>
{
    public override async Task<IDatabaseInserter<UnitedStatesCongressionalMeeting>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "united_states_congressional_meeting",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = UnitedStatesCongressionalMeetingInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = UnitedStatesCongressionalMeetingInserter.DATE_RANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
                new ColumnDefinition{
                    Name = UnitedStatesCongressionalMeetingInserter.NUMBER,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new UnitedStatesCongressionalMeetingInserter(command);
    }

}
internal sealed class UnitedStatesCongressionalMeetingInserter : DatabaseInserter<UnitedStatesCongressionalMeeting>
{

    internal const string ID = "id";
    internal const string DATE_RANGE = "date_range";
    internal const string NUMBER = "number";

    internal UnitedStatesCongressionalMeetingInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(UnitedStatesCongressionalMeeting unitedStatesCongressionalMeeting)
    {
        if (unitedStatesCongressionalMeeting.Id is null)
            throw new NullReferenceException();
        SetParameter(unitedStatesCongressionalMeeting.Id, ID);
        SetDateTimeRangeParameter(unitedStatesCongressionalMeeting.DateRange, DATE_RANGE);
        SetParameter(unitedStatesCongressionalMeeting.Number, NUMBER);
        await _command.ExecuteNonQueryAsync();
    }

}
