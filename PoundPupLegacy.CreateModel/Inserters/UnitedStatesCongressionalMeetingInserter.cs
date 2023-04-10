namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class UnitedStatesCongressionalMeetingInserterFactory : DatabaseInserterFactory<UnitedStatesCongressionalMeeting, UnitedStatesCongressionalMeetingInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter Number = new() { Name = "number" };

    public override string TableName => "united_states_congressional_meeting";
}
internal sealed class UnitedStatesCongressionalMeetingInserter : DatabaseInserter<UnitedStatesCongressionalMeeting>
{
    public UnitedStatesCongressionalMeetingInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(UnitedStatesCongressionalMeeting item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(UnitedStatesCongressionalMeetingInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(UnitedStatesCongressionalMeetingInserterFactory.DateRange, item.DateRange),
            ParameterValue.Create(UnitedStatesCongressionalMeetingInserterFactory.Number, item.Number),
        };
    }
}
