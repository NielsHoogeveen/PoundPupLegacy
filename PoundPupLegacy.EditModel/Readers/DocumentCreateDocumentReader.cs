namespace PoundPupLegacy.EditModel.Readers;

internal sealed class DocumentCreateDocumentReaderFactory : NodeCreateDocumentReaderFactory<Document.ToCreate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.DOCUMENT;

    private const string SQL = $"""
            {SharedSql.SIMPLE_TEXT_NODE_CREATE_CTE},
            {SharedSql.DOCUMENT_TYPES},
            {SharedSql.DOCUMENT_TYPES_DOCUMENT_CREATE}
            select
                jsonb_build_object(
                    'NodeDetailsForCreate',
                    (select document from node_details_for_create_document),
                    'SimpleTextNodeDetails',
                    json_build_object(
                        'Text', 
                        ''
                    ),
                    'DocumentDetails',
                    jsonb_build_object(
        	            'DocumentTypeId',
                        (select id from document_types where is_selected = true),
        	            'PublicationDateFrom',
        	            null,
        	            'PublicationDateTo',
        	            null,
                        'DocumentTypes',
                        (select document from document_types_document)
                    )
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;
}
