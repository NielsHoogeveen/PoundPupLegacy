namespace PoundPupLegacy.EditModel.Readers;

public abstract class SimpleTextNodeCreateDocumentReaderFactory<TResponse> : NodeCreateDocumentReaderFactory<TResponse>
where TResponse : class, SimpleTextNode
{
    public override string Sql => SQL;  
    private const string SQL = $"""
            {CTE_CREATE}
            select
                jsonb_build_object(
                    'NodeId', 
                    null,
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
        """;

}

