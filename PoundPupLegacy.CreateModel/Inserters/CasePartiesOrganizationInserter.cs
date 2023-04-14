namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = CasePartiesOrganizationInserterFactory;
using Request = CasePartiesOrganization;
using Inserter = CasePartiesOrganizationInserter;

internal sealed class CasePartiesOrganizationInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter CasePartiesId = new() { Name = "case_parties_id" };
    internal static NonNullableIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };

    public override string TableName => "case_parties_organization";
}
internal sealed class CasePartiesOrganizationInserter : DatabaseInserter<Request>
{
    public CasePartiesOrganizationInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.CasePartiesId, request.CasePartiesId),
            ParameterValue.Create(Factory.OrganizationId, request.OrganizationId)
        };
    }
}
