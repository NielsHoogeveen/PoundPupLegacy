using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

using Request = OrganizationOrganizationType;

internal sealed class OrganizationOrganizationTypeInserterFactory : BasicDatabaseInserterFactory<Request>
{
    private static readonly NullCheckingIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };
    private static readonly NonNullableIntegerDatabaseParameter OrganizationTypeId = new() { Name = "organization_type_id" };

    public override string TableName => "organization_organization_type";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(OrganizationId, request.OrganizationId),
            ParameterValue.Create(OrganizationTypeId, request.OrganizationTypeId),
        };
    }
}
