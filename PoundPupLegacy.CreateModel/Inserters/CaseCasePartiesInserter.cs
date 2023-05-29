namespace PoundPupLegacy.CreateModel.Inserters;

using Request = CaseExistingCasePartiesToCreate;

internal sealed class CaseCasePartiesInserterFactory : BasicDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter CaseId = new() { Name = "case_id" };
    private static readonly NullCheckingIntegerDatabaseParameter CasePartiesId = new() { Name = "case_parties_id" };
    private static readonly NonNullableIntegerDatabaseParameter CasePartyTypeId = new() { Name = "case_party_type_id" };

    public override string TableName => "case_case_parties";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CaseId, request.CaseId),
            ParameterValue.Create(CasePartiesId, request.CaseParties?.Id),
            ParameterValue.Create(CasePartyTypeId, request.CasePartyTypeId)
        };
    }
}
