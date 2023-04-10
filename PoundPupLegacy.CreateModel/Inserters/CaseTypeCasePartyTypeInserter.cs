namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CaseTypeCasePartyTypeInserterFactory : DatabaseInserterFactory<CaseTypeCasePartyType, CaseTypeCasePartyTypeInserter>
{
    internal static NonNullableIntegerDatabaseParameter CaseTypeId = new() { Name = "case_type_id" };
    internal static NonNullableIntegerDatabaseParameter CasePartyTypeId = new() { Name = "case_party_type_id" };

    public override string TableName => "case_type_case_party_type";

}
internal sealed class CaseTypeCasePartyTypeInserter : DatabaseInserter<CaseTypeCasePartyType>
{
    public CaseTypeCasePartyTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(CaseTypeCasePartyType item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CaseTypeCasePartyTypeInserterFactory.CaseTypeId, item.CaseTypeId),
            ParameterValue.Create(CaseTypeCasePartyTypeInserterFactory.CasePartyTypeId, item.CasePartyTypeId),
        };
    }
}
