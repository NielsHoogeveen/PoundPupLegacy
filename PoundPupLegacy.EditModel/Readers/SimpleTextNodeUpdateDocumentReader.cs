namespace PoundPupLegacy.EditModel.Readers;

public abstract class SimpleTextNodeUpdateDocumentReaderFactory<TResponse> : NodeUpdateDocumentReaderFactory<TResponse>
where TResponse : class, SimpleTextNode
{
    public override string Sql => SQL;
    protected const string SQL = $"""
            {CTE_EDIT}
            select
                jsonb_build_object(
                    'NodeId', 
                    n.id,
                    'NodeTypeName',
                    nt.name,
                    'UrlId', 
                    tn.url_id,
                    'PublisherId', 
                    n.publisher_id,
                    'OwnerId', 
                    n.owner_id,
                    'Title', 
                    n.title,
                    'Text', 
                    stn.text,
            		'Tags', 
                    (select document from tags_document),
                    'TenantNodes',
                    (select document from tenant_nodes_document),
                    'Tenants',
                    (select document from tenants_document),
                    'Files',
                    (select document from attachments_document)
                ) document
            from node n
            join node_type nt on nt.id = n.node_type_id
            join simple_text_node stn on stn.id = n.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id and n.node_type_id = @node_type_id
        """;
}

