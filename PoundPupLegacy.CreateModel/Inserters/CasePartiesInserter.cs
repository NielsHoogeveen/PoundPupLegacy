namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CasePartiesInserterFactory : AutoGenerateIdDatabaseInserterFactory<CaseParties, CasePartiesInserter>
{
    internal static NullableStringDatabaseParameter Organizations = new() { Name = "organizations" };
    internal static NullableStringDatabaseParameter Persons = new() { Name = "persons" };
    public override string TableName => "case_parties";
}
internal sealed class CasePartiesInserter : AutoGenerateIdDatabaseInserter<CaseParties>
{
    public CasePartiesInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(CaseParties item)
    {
        if (item.Id.HasValue) {
            throw new Exception($"case parties id should be null upon creation");
        }
        return new ParameterValue[] {
            ParameterValue.Create(CasePartiesInserterFactory.Organizations, item.Organizations),
            ParameterValue.Create(CasePartiesInserterFactory.Persons, item.Persons)
        };
    }
}
