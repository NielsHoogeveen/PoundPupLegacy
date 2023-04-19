namespace PoundPupLegacy.CreateModel.Inserters;

using Request = UnitedStatesCongressionalMeeting;

internal sealed class UnitedStatesCongressionalMeetingInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    private static readonly NonNullableIntegerDatabaseParameter Number = new() { Name = "number" };

    public override string TableName => "united_states_congressional_meeting";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(DateRange, request.DateRange),
            ParameterValue.Create(Number, request.Number),
        };
    }
}
