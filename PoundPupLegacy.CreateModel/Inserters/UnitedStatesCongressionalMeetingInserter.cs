namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = UnitedStatesCongressionalMeetingInserterFactory;
using Request = UnitedStatesCongressionalMeeting;
using Inserter = UnitedStatesCongressionalMeetingInserter;

internal sealed class UnitedStatesCongressionalMeetingInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter Number = new() { Name = "number" };

    public override string TableName => "united_states_congressional_meeting";
}
internal sealed class UnitedStatesCongressionalMeetingInserter : IdentifiableDatabaseInserter<Request>
{
    public UnitedStatesCongressionalMeetingInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.DateRange, request.DateRange),
            ParameterValue.Create(Factory.Number, request.Number),
        };
    }
}
