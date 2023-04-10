namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CasePartiesOrganizationInserterFactory : DatabaseInserterFactory<CasePartiesOrganization, CasePartiesOrganizationInserter>
{
    internal static NonNullableIntegerDatabaseParameter CasePartiesId = new() { Name = "case_parties_id" };
    internal static NonNullableIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };

    public override string TableName => "case_parties_organization";
}
internal sealed class CasePartiesOrganizationInserter : DatabaseInserter<CasePartiesOrganization>
{
    public CasePartiesOrganizationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(CasePartiesOrganization item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CasePartiesOrganizationInserterFactory.CasePartiesId, item.CasePartiesId),
            ParameterValue.Create(CasePartiesOrganizationInserterFactory.OrganizationId, item.OrganizationId)
        };
    }
}
