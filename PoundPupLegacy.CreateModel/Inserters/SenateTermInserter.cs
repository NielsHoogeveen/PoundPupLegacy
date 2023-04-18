namespace PoundPupLegacy.CreateModel.Inserters;

using Request = SenateTerm;

internal sealed class SenateTermInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NullCheckingIntegerDatabaseParameter SenatorId = new() { Name = "senator_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    internal static NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };

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
