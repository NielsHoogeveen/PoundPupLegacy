using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Inserters;

using Request = OrganizationOrganizationTypeInserterRequest;

public record OrganizationOrganizationTypeInserterRequest: IRequest
{
    public required int OrganizationId { get; init; }

    public required int OrganizationTypeId { get; init; }
}

public sealed class OrganizationOrganizationTypeInserterFactory : DatabaseInserterFactoryBase<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };
    private static readonly NonNullableIntegerDatabaseParameter OrganizationTypeId = new() { Name = "organization_type_id" };

    private IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(OrganizationId, request.OrganizationId),
            ParameterValue.Create(OrganizationTypeId, request.OrganizationTypeId),
        };
    }

    protected override IDatabaseInserter<Request> CreateInstance(NpgsqlCommand command)
    {
        return new BasicDatabaseInserter<Request>(command, GetParameterValues);
    }

    protected override string Sql => SQL;

    const string SQL = $"""
        insert into organization_organization_type (organization_id, organization_type_id) VALUES(@organization_id, @organization_type_id);
        """;
}

