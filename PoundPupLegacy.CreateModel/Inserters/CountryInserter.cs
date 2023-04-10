namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CountryInserterFactory : DatabaseInserterFactory<Country>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter HagueStatusId = new() { Name = "hague_status_id" };
    internal static NullableStringDatabaseParameter ResidencyRequirements = new() { Name = "residency_requirements" };
    internal static NullableStringDatabaseParameter AgeRequirements = new() { Name = "age_requirements" };
    internal static NullableStringDatabaseParameter MarriageRequirements = new() { Name = "marriage_requirements" };
    internal static NullableStringDatabaseParameter IncomeRequirements = new() { Name = "income_requirements" };
    internal static NullableStringDatabaseParameter HealthRequirements = new() { Name = "health_requirements" };
    internal static NullableStringDatabaseParameter OtherRequirements = new() { Name = "other_requirements" };

    public override async Task<IDatabaseInserter<Country>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "country",
            new DatabaseParameter[] {
                Id,
                HagueStatusId,
                ResidencyRequirements,
                AgeRequirements,
                MarriageRequirements,
                IncomeRequirements,
                HealthRequirements,
                OtherRequirements
            }
        );
        return new CountryInserter(command);

    }

}
internal sealed class CountryInserter : DatabaseInserter<Country>
{

    internal CountryInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Country country)
    {
        if (country.Id is null)
            throw new NullReferenceException();

        Set(CountryInserterFactory.Id, country.Id.Value);
        Set(CountryInserterFactory.HagueStatusId, country.HagueStatusId);
        Set(CountryInserterFactory.ResidencyRequirements, country.ResidencyRequirements);
        Set(CountryInserterFactory.AgeRequirements, country.AgeRequirements);
        Set(CountryInserterFactory.MarriageRequirements, country.MarriageRequirements);
        Set(CountryInserterFactory.IncomeRequirements, country.IncomeRequirements);
        Set(CountryInserterFactory.HealthRequirements, country.HealthRequirements);
        Set(CountryInserterFactory.OtherRequirements, country.OtherRequirements);
        await _command.ExecuteNonQueryAsync();
    }
}
