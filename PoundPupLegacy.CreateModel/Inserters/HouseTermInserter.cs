namespace PoundPupLegacy.CreateModel.Inserters;

using Request = HouseTerm.ToCreate;

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
            ParameterValue.Create(RepresentativeId, request.HouseTermDetails.RepresentativeId),
            ParameterValue.Create(SubdivisionId, request.HouseTermDetails.SubdivisionId),
            ParameterValue.Create(District, request.HouseTermDetails.District),
            ParameterValue.Create(DateRange, request.HouseTermDetails.DateTimeRange),
        };
    }
}
