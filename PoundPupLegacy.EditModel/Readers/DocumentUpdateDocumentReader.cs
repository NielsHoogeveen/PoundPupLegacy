namespace PoundPupLegacy.EditModel.Readers;

internal sealed class DocumentUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<Document.ExistingDocument>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.DOCUMENT;

    const string SQL = $"""
        {CTE_EDIT}
        select
            jsonb_build_object(
        	    'NodeId',
        	    d.id,
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
        	    'SourceUrl',
        	    d.source_url,
        	    'Text',
        	    stn.text,
        	    'DocumentTypeId',
                case 
                    when d.document_type_id is null then 0
                    else d.document_type_id
                end,
        	    'PublicationDateFrom',
        	    lower(published),
        	    'PublicationDateTo',
        	    upper(published),
                'DocumentTypes',
                (select document from document_types_document),
                'TenantNodes',
                (select document from tenant_nodes_document),
                'Tenants',
                (select document from tenants_document),
                'Files',
                (select document from attachments_document),
                'Tags',
                (select document from tags_document)
            ) document
        from document d
        join simple_text_node stn on stn.id = d.id
        join node n on n.id = d.id
        join node_type nt on nt.id = n.node_type_id
        join tenant_node tn on tn.node_id = d.id
        where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        """;

}
