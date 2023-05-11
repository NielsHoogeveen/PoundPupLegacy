namespace PoundPupLegacy.EditModel.Readers;

internal sealed class PersonCreateDocumentReaderFactory : NodeCreateDocumentReaderFactory<Person>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.PERSON;

    private const string SQL = $"""
            {CTE_CREATE}
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
                    'Description', 
                    '',
            		'Tags', null,
                    'TenantNodes',
                    null,
                    'Tenants',
                    (select document from tenants_document),
                    'Files',
                    null,
                    'Tags',
                    (select document from tags_document)
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;
}
