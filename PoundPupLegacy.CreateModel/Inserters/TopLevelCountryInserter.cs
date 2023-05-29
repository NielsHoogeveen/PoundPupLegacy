namespace PoundPupLegacy.CreateModel.Inserters;

using Request = TopLevelCountryToCreate;

internal sealed class TopLevelCountryInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableFixedStringDatabaseParameter ISO3166_1_code = new() { Name = "iso_3166_1_code" };
    private static readonly NonNullableIntegerDatabaseParameter GlobalRegionId = new() { Name = "global_region_id" };
    public override string TableName => "top_level_country";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(ISO3166_1_code, request.TopLevelCountryDetails.ISO3166_1_Code),
            ParameterValue.Create(GlobalRegionId, request.TopLevelCountryDetails.SecondLevelRegionId),
        };
    }
}
