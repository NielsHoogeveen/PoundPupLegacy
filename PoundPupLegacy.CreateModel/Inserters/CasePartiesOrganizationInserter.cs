namespace PoundPupLegacy.CreateModel.Inserters;

using Request = CasePartiesOrganization;

internal sealed class CasePartiesOrganizationInserterFactory : BasicDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter CasePartiesId = new() { Name = "case_parties_id" };
    private static readonly NonNullableIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };

    public override string TableName => "case_parties_organization";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CasePartiesId, request.CasePartiesId),
            ParameterValue.Create(OrganizationId, request.OrganizationId)
        };
    }
}
