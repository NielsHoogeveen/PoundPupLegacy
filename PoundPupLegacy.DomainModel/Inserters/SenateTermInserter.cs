namespace PoundPupLegacy.DomainModel.Inserters;

using Request = SenateTerm.ToCreateForExistingSenator;

internal sealed class SenateTermInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullCheckingIntegerDatabaseParameter SenatorId = new() { Name = "senator_id" };
    private static readonly NonNullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    private static readonly NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };

    public override string TableName => "senate_term";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(SenatorId, request.SenateTermDetails.SenatorId),
            ParameterValue.Create(SubdivisionId, request.SenateTermDetails.SubdivisionId),
            ParameterValue.Create(DateRange, request.SenateTermDetails.DateTimeRange),
        };
    }
}
