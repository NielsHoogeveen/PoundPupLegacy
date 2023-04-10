namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CaseCasePartiesInserterFactory : BasicDatabaseInserterFactory<CaseCaseParties, CaseCasePartiesInserter>
{
    internal static NonNullableIntegerDatabaseParameter CaseId = new() { Name = "case_id" };
    internal static NonNullableIntegerDatabaseParameter CasePartiesId = new() { Name = "case_parties_id" };
    internal static NonNullableIntegerDatabaseParameter CasePartyTypeId = new() { Name = "case_party_type_id" };

    public override string TableName => "case_case_parties";
}
internal sealed class CaseCasePartiesInserter : BasicDatabaseInserter<CaseCaseParties>
{

    public CaseCasePartiesInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(CaseCaseParties item)
    {
        if (item.CaseParties.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(CaseCasePartiesInserterFactory.CaseId, item.CaseId),
            ParameterValue.Create(CaseCasePartiesInserterFactory.CasePartiesId, item.CaseParties.Id.Value),
            ParameterValue.Create(CaseCasePartiesInserterFactory.CasePartyTypeId, item.CasePartyTypeId)
        };
    }

}
