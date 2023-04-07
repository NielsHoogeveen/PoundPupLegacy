using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Updaters;

internal sealed class TenantNodeUpdaterFactory : DatabaseUpdaterFactory<TenantNodeUpdater>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() {
        Name = "id"
    };
    internal static NullableStringDatabaseParameter UrlPath = new() {
        Name = "url_path"
    };
    internal static NullableIntegerDatabaseParameter SubgroupId = new() {
        Name = "subgroup_id"
    };
    internal static NonNullableIntegerDatabaseParameter PublicationStatusId = new() {
        Name = "publication_status_id"
    };

    public override string Sql => $"""
        update tenant_node 
        set 
        url_path = @url_path, 
        subgroup_id = @subgroup_id, 
        publication_status_id = @publication_status_id
        where id = @id
        """;
}

    

internal sealed class TenantNodeUpdater : DatabaseUpdater<TenantNodeUpdater.Request>
{

    public TenantNodeUpdater(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(TenantNodeUpdaterFactory.Id, request.Id),
            ParameterValue.Create(TenantNodeUpdaterFactory.UrlPath, request.UrlPath),
            ParameterValue.Create(TenantNodeUpdaterFactory.SubgroupId, request.SubgroupId),
            ParameterValue.Create(TenantNodeUpdaterFactory.PublicationStatusId, request.PublicationStatusId)
        };
    }

    public record Request
    {
        public required int Id { get; init; }
        public required string? UrlPath { get; init; }
        public required int? SubgroupId { get; init; }
        public required int PublicationStatusId { get; init; }
    }
}
