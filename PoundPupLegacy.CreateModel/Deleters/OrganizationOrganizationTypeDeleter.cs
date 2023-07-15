namespace PoundPupLegacy.DomainModel.Deleters;

using Request = OrganizationOrganizationTypeDeleterRequest;

public sealed record OrganizationOrganizationTypeDeleterRequest : IRequest
{
    public required int LocationId { get; init; }
    public required int LocatableId { get; init; }
}

internal sealed class OrganizationOrganizationTypeDeleterFactory : DatabaseDeleterFactory<Request>
{
    private static NonNullableIntegerDatabaseParameter Organization = new() { Name = "organization_id" };
    private static NonNullableIntegerDatabaseParameter organizationTypeId = new() { Name = "organization_type_id" };

    public override string Sql => SQL;

    const string SQL = $"""
        delete from organization_organization_type
        where organization_id = @organization_id and organization_type_id = @organization_type_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Organization, request.LocationId),
            ParameterValue.Create(organizationTypeId, request.LocatableId),
        };
    }
}
