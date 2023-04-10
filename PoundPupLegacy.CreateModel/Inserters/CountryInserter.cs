namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CountryInserterFactory : BasicDatabaseInserterFactory<Country, CountryInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter HagueStatusId = new() { Name = "hague_status_id" };
    internal static NullableStringDatabaseParameter ResidencyRequirements = new() { Name = "residency_requirements" };
    internal static NullableStringDatabaseParameter AgeRequirements = new() { Name = "age_requirements" };
    internal static NullableStringDatabaseParameter MarriageRequirements = new() { Name = "marriage_requirements" };
    internal static NullableStringDatabaseParameter IncomeRequirements = new() { Name = "income_requirements" };
    internal static NullableStringDatabaseParameter HealthRequirements = new() { Name = "health_requirements" };
    internal static NullableStringDatabaseParameter OtherRequirements = new() { Name = "other_requirements" };

    public override string TableName => "country";

}
internal sealed class CountryInserter : BasicDatabaseInserter<Country>
{

    public CountryInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(Country item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(CountryInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(CountryInserterFactory.HagueStatusId, item.HagueStatusId),
            ParameterValue.Create(CountryInserterFactory.ResidencyRequirements, item.ResidencyRequirements),
            ParameterValue.Create(CountryInserterFactory.AgeRequirements, item.AgeRequirements),
            ParameterValue.Create(CountryInserterFactory.MarriageRequirements, item.MarriageRequirements),
            ParameterValue.Create(CountryInserterFactory.IncomeRequirements, item.IncomeRequirements),
            ParameterValue.Create(CountryInserterFactory.HealthRequirements, item.HealthRequirements),
            ParameterValue.Create(CountryInserterFactory.OtherRequirements, item.OtherRequirements),
        };
    }
}
