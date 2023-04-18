namespace PoundPupLegacy.CreateModel.Inserters;

using Request = HouseTerm;

internal sealed class HouseTermInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NullCheckingIntegerDatabaseParameter RepresentativeId = new() { Name = "representative_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    internal static NullableIntegerDatabaseParameter District = new() { Name = "district" };
    internal static NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };

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
