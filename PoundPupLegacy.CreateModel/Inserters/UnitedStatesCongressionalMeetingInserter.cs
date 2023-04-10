namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class UnitedStatesCongressionalMeetingInserterFactory : DatabaseInserterFactory<UnitedStatesCongressionalMeeting>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter Number = new() { Name = "number" };

    public override async Task<IDatabaseInserter<UnitedStatesCongressionalMeeting>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "united_states_congressional_meeting",
            new DatabaseParameter[] {
                Id,
                DateRange,
                Number
            }
        );
        return new UnitedStatesCongressionalMeetingInserter(command);
    }

}
internal sealed class UnitedStatesCongressionalMeetingInserter : DatabaseInserter<UnitedStatesCongressionalMeeting>
{

    internal UnitedStatesCongressionalMeetingInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(UnitedStatesCongressionalMeeting unitedStatesCongressionalMeeting)
    {
        if (unitedStatesCongressionalMeeting.Id is null)
            throw new NullReferenceException();
        Set(UnitedStatesCongressionalMeetingInserterFactory.Id, unitedStatesCongressionalMeeting.Id.Value);
        Set(UnitedStatesCongressionalMeetingInserterFactory.DateRange, unitedStatesCongressionalMeeting.DateRange);
        Set(UnitedStatesCongressionalMeetingInserterFactory.Number, unitedStatesCongressionalMeeting.Number);
        await _command.ExecuteNonQueryAsync();
    }

}
