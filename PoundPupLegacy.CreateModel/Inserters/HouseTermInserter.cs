namespace PoundPupLegacy.CreateModel.Inserters;

using Request = EventuallyIdentifiableHouseTerm;

internal sealed class HouseTermInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullCheckingIntegerDatabaseParameter RepresentativeId = new() { Name = "representative_id" };
    private static readonly NonNullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    private static readonly NullableIntegerDatabaseParameter District = new() { Name = "district" };
    private static readonly NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };

    public override string TableName => "house_term";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(RepresentativeId, request.RepresentativeId),
            ParameterValue.Create(SubdivisionId, request.SubdivisionId),
            ParameterValue.Create(District, request.District),
            ParameterValue.Create(DateRange, request.DateTimeRange),
        };
    }
}
