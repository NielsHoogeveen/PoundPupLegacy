namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = CountryInserterFactory;
using Request = Country;
using Inserter = CountryInserter;

internal sealed class CountryInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter HagueStatusId = new() { Name = "hague_status_id" };
    internal static NullableStringDatabaseParameter ResidencyRequirements = new() { Name = "residency_requirements" };
    internal static NullableStringDatabaseParameter AgeRequirements = new() { Name = "age_requirements" };
    internal static NullableStringDatabaseParameter MarriageRequirements = new() { Name = "marriage_requirements" };
    internal static NullableStringDatabaseParameter IncomeRequirements = new() { Name = "income_requirements" };
    internal static NullableStringDatabaseParameter HealthRequirements = new() { Name = "health_requirements" };
    internal static NullableStringDatabaseParameter OtherRequirements = new() { Name = "other_requirements" };

    public override string TableName => "country";

}
internal sealed class CountryInserter : IdentifiableDatabaseInserter<Request>
{

    public CountryInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.HagueStatusId, request.HagueStatusId),
            ParameterValue.Create(Factory.ResidencyRequirements, request.ResidencyRequirements),
            ParameterValue.Create(Factory.AgeRequirements, request.AgeRequirements),
            ParameterValue.Create(Factory.MarriageRequirements, request.MarriageRequirements),
            ParameterValue.Create(Factory.IncomeRequirements, request.IncomeRequirements),
            ParameterValue.Create(Factory.HealthRequirements, request.HealthRequirements),
            ParameterValue.Create(Factory.OtherRequirements, request.OtherRequirements),
        };
    }
}
