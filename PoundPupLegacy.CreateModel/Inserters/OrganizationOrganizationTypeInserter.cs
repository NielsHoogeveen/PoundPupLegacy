namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class OrganizationOrganizationTypeInserterFactory : DatabaseInserterFactory<OrganizationOrganizationType, OrganizationOrganizationTypeInserter>
{
    internal static NonNullableIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };
    internal static NonNullableIntegerDatabaseParameter OrganizationTypeId = new() { Name = "organization_type_id" };

    public override string TableName => "organization_organization_type";

}
internal sealed class OrganizationOrganizationTypeInserter : DatabaseInserter<OrganizationOrganizationType>
{
    public OrganizationOrganizationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(OrganizationOrganizationType item)
    {
        if (item.OrganizationId is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(OrganizationOrganizationTypeInserterFactory.OrganizationId, item.OrganizationId.Value),
            ParameterValue.Create(OrganizationOrganizationTypeInserterFactory.OrganizationTypeId, item.OrganizationTypeId),
        };
    }
}
