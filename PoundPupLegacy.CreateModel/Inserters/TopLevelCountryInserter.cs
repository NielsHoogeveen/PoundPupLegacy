namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = TopLevelCountryInserterFactory;
using Request = TopLevelCountry;
using Inserter = TopLevelCountryInserter;

internal sealed class TopLevelCountryInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableFixedStringDatabaseParameter ISO3166_1_code = new() { Name = "iso_3166_1_code" };
    internal static NonNullableIntegerDatabaseParameter GlobalRegionId = new() { Name = "global_region_id" };
    public override string TableName => "top_level_country";
}
internal sealed class TopLevelCountryInserter : IdentifiableDatabaseInserter<Request>
{
    public TopLevelCountryInserter(NpgsqlCommand command) : base(command)
    {
    }
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.ISO3166_1_code, request.ISO3166_1_Code),
            ParameterValue.Create(Factory.GlobalRegionId, request.SecondLevelRegionId),
        };
    }
}
