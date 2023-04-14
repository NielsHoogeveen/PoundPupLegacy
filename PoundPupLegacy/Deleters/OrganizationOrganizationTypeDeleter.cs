using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Deleters;

using Request = OrganizationOrganizationTypeDeleterRequest;
using Factory = OrganizationOrganizationTypeDeleterFactory;
using Deleter = OrganizationOrganizationTypeDeleter;

public record OrganizationOrganizationTypeDeleterRequest: IRequest
{
    public required int LocationId { get; init; }
    public required int LocatableId { get; init; }
}

internal sealed class OrganizationOrganizationTypeDeleterFactory : DatabaseDeleterFactory<Request,Deleter>
{
    internal static NonNullableIntegerDatabaseParameter Organization = new() { Name = "organization_id" };
    internal static NonNullableIntegerDatabaseParameter organizationTypeId = new() { Name = "organization_type_id" };

    public override string Sql => SQL;

    const string SQL = $"""
        delete from organization_organization_type
        where organization_id = @organization_id and organization_type_id = @organization_type_id;
        """;
}
internal sealed class OrganizationOrganizationTypeDeleter : DatabaseDeleter<Request>
{
    public OrganizationOrganizationTypeDeleter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Organization, request.LocationId),
            ParameterValue.Create(Factory.organizationTypeId, request.LocatableId),
        };
    }
}
