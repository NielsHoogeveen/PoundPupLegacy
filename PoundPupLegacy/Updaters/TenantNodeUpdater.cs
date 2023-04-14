using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Updaters;

using Request = TenantNodeUpdaterRequest;
using Factory = TenantNodeUpdaterFactory;
using Updater = TenantNodeUpdater;

public record TenantNodeUpdaterRequest: IRequest
{
    public required int Id { get; init; }
    public required string? UrlPath { get; init; }
    public required int? SubgroupId { get; init; }
    public required int PublicationStatusId { get; init; }
}

internal sealed class TenantNodeUpdaterFactory : DatabaseUpdaterFactory<Request,Updater>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableStringDatabaseParameter UrlPath = new() { Name = "url_path" };
    internal static NullableIntegerDatabaseParameter SubgroupId = new() { Name = "subgroup_id" };
    internal static NonNullableIntegerDatabaseParameter PublicationStatusId = new() { Name = "publication_status_id" };

    public override string Sql => $"""
        update tenant_node 
        set 
        url_path = @url_path, 
        subgroup_id = @subgroup_id, 
        publication_status_id = @publication_status_id
        where id = @id
        """;
}

internal sealed class TenantNodeUpdater : DatabaseUpdater<Request>
{
    public TenantNodeUpdater(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(Factory.Id, request.Id),
            ParameterValue.Create(Factory.UrlPath, request.UrlPath),
            ParameterValue.Create(Factory.SubgroupId, request.SubgroupId),
            ParameterValue.Create(Factory.PublicationStatusId, request.PublicationStatusId)
        };
    }
}
