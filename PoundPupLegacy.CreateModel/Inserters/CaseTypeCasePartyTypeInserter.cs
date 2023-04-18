namespace PoundPupLegacy.CreateModel.Inserters;

using Request = CaseTypeCasePartyType;

internal sealed class CaseTypeCasePartyTypeInserterFactory : BasicDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter CaseTypeId = new() { Name = "case_type_id" };
    internal static NonNullableIntegerDatabaseParameter CasePartyTypeId = new() { Name = "case_party_type_id" };

    public override string TableName => "case_type_case_party_type";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CaseTypeId, request.CaseTypeId),
            ParameterValue.Create(CasePartyTypeId, request.CasePartyTypeId),
        };
    }
}
