namespace PoundPupLegacy.CreateModel.Updaters;

using Request = TenantNode.ToUpdate;

internal sealed class TenantNodeUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    private static readonly NullableStringDatabaseParameter UrlPath = new() { Name = "url_path" };
    private static readonly NullableIntegerDatabaseParameter SubgroupId = new() { Name = "subgroup_id" };
    private static readonly NonNullableIntegerDatabaseParameter PublicationStatusId = new() { Name = "publication_status_id" };

    public override string Sql => $"""
        update tenant_node 
        set 
        url_path = @url_path, 
        subgroup_id = @subgroup_id, 
        publication_status_id = @publication_status_id
        where id = @id
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(Id, request.Identification.Id),
            ParameterValue.Create(UrlPath, request.UrlPath),
            ParameterValue.Create(SubgroupId, request.SubgroupId),
            ParameterValue.Create(PublicationStatusId, request.PublicationStatusId)
        };
    }
}

