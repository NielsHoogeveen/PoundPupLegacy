namespace PoundPupLegacy.EditModel.Readers;

using Reader = DocumentUpdateDocumentReader;

public class DocumentUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<Reader>
{
    public override string Sql => SQL;

    const string SQL = $"""
        {CTE_EDIT}
        select
            jsonb_build_object(
        	    'NodeId',
        	    d.id,
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
        	    d.text,
        	    'DocumentTypeId',
                case 
                    when d.document_type_id is null then 0
                    else d.document_type_id
                end,
        	    'PublicationDateFrom',
        	    lower(published),
        	    'PublicationDateTo',
        	    upper(published),
                'DocumentableDocuments',
                (select document from document_documentables_document),
                'DocumentTypes',
                (select document from document_types_document),
                'TenantNodes',
                (select document from tenant_nodes_document),
                'Tenants',
                (select document from tenants_document),
                'Files',
                (select document from attachments_document)
            ) document
        from document d
        join node n on n.id = d.id
        join tenant_node tn on tn.node_id = d.id
        where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        """;

}

public class DocumentUpdateDocumentReader : NodeUpdateDocumentReader<Document>
{
    internal DocumentUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.DOCUMENT)
    {
    }
}


