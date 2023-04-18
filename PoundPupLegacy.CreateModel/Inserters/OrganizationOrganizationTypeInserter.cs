namespace PoundPupLegacy.CreateModel.Inserters;

using Request = OrganizationOrganizationType;

internal sealed class OrganizationOrganizationTypeInserterFactory : BasicDatabaseInserterFactory<Request>
{
    internal static NullCheckingIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };
    internal static NonNullableIntegerDatabaseParameter OrganizationTypeId = new() { Name = "organization_type_id" };

    public override string TableName => "organization_organization_type";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(OrganizationId, request.OrganizationId),
            ParameterValue.Create(OrganizationTypeId, request.OrganizationTypeId),
        };
    }
}
