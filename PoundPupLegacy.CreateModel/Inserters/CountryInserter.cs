namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Country;

internal sealed class CountryInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter HagueStatusId = new() { Name = "hague_status_id" };
    private static readonly NullableStringDatabaseParameter ResidencyRequirements = new() { Name = "residency_requirements" };
    private static readonly NullableStringDatabaseParameter AgeRequirements = new() { Name = "age_requirements" };
    private static readonly NullableStringDatabaseParameter MarriageRequirements = new() { Name = "marriage_requirements" };
    private static readonly NullableStringDatabaseParameter IncomeRequirements = new() { Name = "income_requirements" };
    private static readonly NullableStringDatabaseParameter HealthRequirements = new() { Name = "health_requirements" };
    private static readonly NullableStringDatabaseParameter OtherRequirements = new() { Name = "other_requirements" };

    public override string TableName => "country";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(HagueStatusId, request.HagueStatusId),
            ParameterValue.Create(ResidencyRequirements, request.ResidencyRequirements),
            ParameterValue.Create(AgeRequirements, request.AgeRequirements),
            ParameterValue.Create(MarriageRequirements, request.MarriageRequirements),
            ParameterValue.Create(IncomeRequirements, request.IncomeRequirements),
            ParameterValue.Create(HealthRequirements, request.HealthRequirements),
            ParameterValue.Create(OtherRequirements, request.OtherRequirements),
        };
    }
}
