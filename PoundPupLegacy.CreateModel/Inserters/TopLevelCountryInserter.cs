namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TopLevelCountryInserterFactory : DatabaseInserterFactory<TopLevelCountry> 
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableFixedStringDatabaseParameter ISO3166_1_code = new() { Name = "iso_3166_1_code" };
    internal static NonNullableIntegerDatabaseParameter GlobalRegionId = new() { Name = "global_region_id" };

    public override async Task<IDatabaseInserter<TopLevelCountry>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "top_level_country",
            new DatabaseParameter[] {
                Id,
                ISO3166_1_code,
                GlobalRegionId
            }
        );
        return new TopLevelCountryInserter(command);
    }
}
internal sealed class TopLevelCountryInserter : DatabaseInserter<TopLevelCountry>
{

    internal TopLevelCountryInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(TopLevelCountry country)
    {
        if (country.Id is null)
            throw new NullReferenceException();
        Set(TopLevelCountryInserterFactory.Id, country.Id.Value);
        Set(TopLevelCountryInserterFactory.ISO3166_1_code, country.ISO3166_1_Code);
        Set(TopLevelCountryInserterFactory.GlobalRegionId, country.SecondLevelRegionId);
        await _command.ExecuteNonQueryAsync();
    }
}
