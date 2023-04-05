namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CountryInserterFactory : DatabaseInserterFactory<Country>
{
    public override async Task<IDatabaseInserter<Country>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "country",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = CountryInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CountryInserter.HAGUE_STATUS_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CountryInserter.RESIDENCY_REQUIREMENTS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = CountryInserter.AGE_REQUIREMENTS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = CountryInserter.MARRIAGE_REQUIREMENTS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = CountryInserter.INCOME_REQUIREMENTS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = CountryInserter.HEALTH_REQUIREMENTS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = CountryInserter.OTHER_REQUIREMENTS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new CountryInserter(command);

    }

}
internal sealed class CountryInserter : DatabaseInserter<Country>
{
    internal const string ID = "id";
    internal const string HAGUE_STATUS_ID = "hague_status_id";
    internal const string RESIDENCY_REQUIREMENTS = "residency_requirements";
    internal const string AGE_REQUIREMENTS = "age_requirements";
    internal const string MARRIAGE_REQUIREMENTS = "marriage_requirements";
    internal const string INCOME_REQUIREMENTS = "income_requirements";
    internal const string HEALTH_REQUIREMENTS = "health_requirements";
    internal const string OTHER_REQUIREMENTS = "other_requirements";

    internal CountryInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Country country)
    {
        if (country.Id is null)
            throw new NullReferenceException();

        WriteValue(country.Id, ID);
        WriteValue(country.HagueStatusId, HAGUE_STATUS_ID);
        WriteNullableValue(country.ResidencyRequirements, RESIDENCY_REQUIREMENTS);
        WriteNullableValue(country.AgeRequirements, AGE_REQUIREMENTS);
        WriteNullableValue(country.MarriageRequirements, MARRIAGE_REQUIREMENTS);
        WriteNullableValue(country.IncomeRequirements, INCOME_REQUIREMENTS);
        WriteNullableValue(country.HealthRequirements, HEALTH_REQUIREMENTS);
        WriteNullableValue(country.OtherRequirements, OTHER_REQUIREMENTS);
        await _command.ExecuteNonQueryAsync();
    }
}
