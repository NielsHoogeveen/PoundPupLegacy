namespace PoundPupLegacy.EditModel.Readers;

internal sealed class DocumentUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<Document.ToUpdate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.DOCUMENT;

    const string SQL = $"""
        {SharedSql.NODE_UPDATE_CTE},
        {SharedSql.DOCUMENT_TYPES},
        {SharedSql.DOCUMENT_TYPES_DOCUMENT_CREATE}
        select
            jsonb_build_object(
                'NodeIdentification', 
                (select document from identification_for_update_document where id = n.id),
                'NodeDetailsForUpdate',
                (select document from node_details_for_update_document where id = n.id),
                'SimpleTextNodeDetails',
                json_build_object(
                    'Text', 
                    stn.text
                ),
                'DocumentDetails',
                jsonb_build_object(
        	        'DocumentTypeId',
                    case 
                        when d.document_type_id is null then 101488
                        else d.document_type_id
                    end,
                    'SourceUrl',
                    d.source_url,
        	        'PublicationDateFrom',
        	        lower(published),
        	        'PublicationDateTo',
        	        upper(published),
                    'DocumentTypes',
                    (select document from document_types_document)
                )
            ) document
        from document d
        join simple_text_node stn on stn.id = d.id
        join node n on n.id = d.id
        join node_type nt on nt.id = n.node_type_id
        join tenant_node tn on tn.node_id = d.id
        where tn.tenant_id = @tenant_id and tn.node_id = @node_id
        """;

}
