namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TopLevelCountryInserterFactory : DatabaseInserterFactory<TopLevelCountry> 
{
    public override async Task<IDatabaseInserter<TopLevelCountry>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "top_level_country",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = TopLevelCountryInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TopLevelCountryInserter.ISO_3166_1_CODE,
                    NpgsqlDbType = NpgsqlDbType.Char
                },
                new ColumnDefinition{
                    Name = TopLevelCountryInserter.GLOBAL_REGION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new TopLevelCountryInserter(command);
    }
}
internal sealed class TopLevelCountryInserter : DatabaseInserter<TopLevelCountry>
{
    internal const string ID = "id";
    internal const string ISO_3166_1_CODE = "iso_3166_1_code";
    internal const string GLOBAL_REGION_ID = "global_region_id";

    internal TopLevelCountryInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(TopLevelCountry country)
    {
        if (country.Id is null)
            throw new NullReferenceException();
        SetParameter(country.Id, ID);
        SetParameter(country.ISO3166_1_Code, ISO_3166_1_CODE);
        SetParameter(country.SecondLevelRegionId, GLOBAL_REGION_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
