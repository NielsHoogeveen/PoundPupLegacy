namespace PoundPupLegacy.CreateModel.Inserters;

using Request = TopLevelCountry;

internal sealed class TopLevelCountryInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableFixedStringDatabaseParameter ISO3166_1_code = new() { Name = "iso_3166_1_code" };
    internal static NonNullableIntegerDatabaseParameter GlobalRegionId = new() { Name = "global_region_id" };
    public override string TableName => "top_level_country";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(ISO3166_1_code, request.ISO3166_1_Code),
            ParameterValue.Create(GlobalRegionId, request.SecondLevelRegionId),
        };
    }
}
