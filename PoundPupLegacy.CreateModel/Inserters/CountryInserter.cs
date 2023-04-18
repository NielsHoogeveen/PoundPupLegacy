namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Country;

internal sealed class CountryInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter HagueStatusId = new() { Name = "hague_status_id" };
    internal static NullableStringDatabaseParameter ResidencyRequirements = new() { Name = "residency_requirements" };
    internal static NullableStringDatabaseParameter AgeRequirements = new() { Name = "age_requirements" };
    internal static NullableStringDatabaseParameter MarriageRequirements = new() { Name = "marriage_requirements" };
    internal static NullableStringDatabaseParameter IncomeRequirements = new() { Name = "income_requirements" };
    internal static NullableStringDatabaseParameter HealthRequirements = new() { Name = "health_requirements" };
    internal static NullableStringDatabaseParameter OtherRequirements = new() { Name = "other_requirements" };

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
