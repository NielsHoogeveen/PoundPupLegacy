namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TopLevelCountryInserterFactory : DatabaseInserterFactory<TopLevelCountry, TopLevelCountryInserter> 
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableFixedStringDatabaseParameter ISO3166_1_code = new() { Name = "iso_3166_1_code" };
    internal static NonNullableIntegerDatabaseParameter GlobalRegionId = new() { Name = "global_region_id" };
    public override string TableName => "top_level_country";
}
internal sealed class TopLevelCountryInserter : DatabaseInserter<TopLevelCountry>
{
    public TopLevelCountryInserter(NpgsqlCommand command) : base(command)
    {
    }
    public override IEnumerable<ParameterValue> GetParameterValues(TopLevelCountry country)
    {
        if (country.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(TopLevelCountryInserterFactory.Id, country.Id.Value),
            ParameterValue.Create(TopLevelCountryInserterFactory.ISO3166_1_code, country.ISO3166_1_Code),
            ParameterValue.Create(TopLevelCountryInserterFactory.GlobalRegionId, country.SecondLevelRegionId),
        };
    }
}
