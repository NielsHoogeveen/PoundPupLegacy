namespace PoundPupLegacy.CreateModel.Inserters;

using Request = SenateTerm;

internal sealed class SenateTermInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullCheckingIntegerDatabaseParameter SenatorId = new() { Name = "senator_id" };
    private static readonly NonNullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    private static readonly NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };

    public override string TableName => "senate_term";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(SenatorId, request.SenatorId),
            ParameterValue.Create(SubdivisionId, request.SubdivisionId),
            ParameterValue.Create(DateRange, request.DateTimeRange),
        };
    }
}
