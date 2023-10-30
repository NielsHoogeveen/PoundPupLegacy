namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using Request = NameablesDocumentReaderRequest;

public sealed record NameablesDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
    public required int NodeTypeId { get; init; }
}
internal sealed class NameableListDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, Nameables>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeTypeIdParameter = new() { Name = "node_type_id" };

    private static readonly FieldValueReader<Nameables> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        with 
        {SharedSql.ACCESSIBLE_PUBLICATIONS_STATUS}
        select 
            json_build_object(
            'NodeTypeName',
            nt.name,
            'NameableListEntries',
            json_agg(
                json_build_object(
                    'Title', 
                    n.title,
                    'Path', 
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else tn.url_path 
                    end,
                    'HasBeenPublished', 
                    tn.publication_status_id <> 0,
                    'PublicationStatusId',
                    tn.publication_status_id
                )
            )
        ) document
        from node n
        join node_type nt on nt.id = n.node_type_id
        join tenant_node tn on tn.node_id = n.id 
        where tn.tenant_id = @tenant_id 
        and tn.url_id = @url_id
        and n.node_type_id = @node_type_id
        and tn.publication_status_id in 
        (
            select 
            id 
            from accessible_publication_status 
            where tenant_id = tn.tenant_id 
            and (
                subgroup_id = tn.subgroup_id 
                or subgroup_id is null and tn.subgroup_id is null
            )
        )
        group by nt.name
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
            ParameterValue.Create(NodeTypeIdParameter, request.NodeTypeId),
        };
    }

    protected override Nameables Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
