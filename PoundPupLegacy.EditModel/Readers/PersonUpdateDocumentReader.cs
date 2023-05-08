namespace PoundPupLegacy.EditModel.Readers;

internal sealed class PersonUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<Person>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.PERSON;

    const string SQL = $"""
            {CTE_EDIT}
            select
                jsonb_build_object(
                    'NodeId', 
                    n.id,
                    'UrlId', 
                    tn.url_id,
                    'PublisherId', 
                    n.publisher_id,
                    'OwnerId', 
                    n.owner_id,
                    'Title' , 
                    n.title,
                    'Description', 
                    nm.description,
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
            join person p on p.id = n.id
            join nameable nm on nm.id = n.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id and n.node_type_id = @node_type_id
        """;

}
