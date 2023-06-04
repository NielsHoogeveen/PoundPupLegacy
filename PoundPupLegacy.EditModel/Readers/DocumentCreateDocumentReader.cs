namespace PoundPupLegacy.EditModel.Readers;

internal sealed class DocumentCreateDocumentReaderFactory : NodeCreateDocumentReaderFactory<Document.ToCreate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.DOCUMENT;

    private const string SQL = $"""
            {SharedSql.SIMPLE_TEXT_NODE_CREATE_CTE},
            {SharedSql.DOCUMENT_TYPES_DOCUMENT_CREATE}
            select
                jsonb_build_object(
                    'NodeId', 
                    null,
                    'NodeTypeName',
                    nt.name,
                    'UrlId', 
                    null,
                    'PublisherId',
                    @user_id,
                    'OwnerId',
                    @tenant_id,
                    'Title', 
                    '',
                    'Text', 
                    '',
            		'Tags', null,
                    'TenantNodes',
                    null,
                    'Tenants',
                    (select document from tenants_document),
                    'Files',
                    null,
                    'DocumentTypes',
                    (select document from document_types_document),
                    'Tags',
                    (select document from tags_document)
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;
}
