namespace PoundPupLegacy.EditModel.Readers;

public abstract class SimpleTextNodeCreateDocumentReaderFactory<TResponse> : NodeCreateDocumentReaderFactory<TResponse>
where TResponse : class, SimpleTextNode, NewNode
{
    public override string Sql => SQL;
    private const string SQL = $"""
            {SharedSql.NODE_CREATE_CTE}
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
                    null
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;

}

