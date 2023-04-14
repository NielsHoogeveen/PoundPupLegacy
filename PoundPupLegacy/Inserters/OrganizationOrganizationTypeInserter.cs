using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Inserters;

using Request = OrganizationOrganizationTypeInserterRequest;
using Factory = OrganizationOrganizationTypeInserterFactory;
using Inserter = OrganizationOrganizationTypeInserter;

public record OrganizationOrganizationTypeInserterRequest: IRequest
{
    public required int OrganizationId { get; init; }

    public required int OrganizationTypeId { get; init; }
}

public sealed class OrganizationOrganizationTypeInserterFactory : DatabaseInserterFactoryBase<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };
    internal static NonNullableIntegerDatabaseParameter OrganizationTypeId = new() { Name = "organization_type_id" };

    protected override string Sql => SQL;

    const string SQL = $"""
        insert into organization_organization_type (organization_id, organization_type_id) VALUES(@organization_id, @organization_type_id);
        """;
}

public sealed class OrganizationOrganizationTypeInserter : DatabaseInserter<Request>
{
    public OrganizationOrganizationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.OrganizationId, request.OrganizationId),
            ParameterValue.Create(Factory.OrganizationTypeId, request.OrganizationTypeId),
        };
    }
}
