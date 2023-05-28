namespace PoundPupLegacy.EditModel.Readers;

internal sealed class WrongfulRemovalCaseCreateDocumentReaderFactory : NodeCreateDocumentReaderFactory<WrongfulRemovalCase.NewWrongfulRemovalCase>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.WRONGFUL_REMOVAL_CASE;

    private const string SQL = $"""
            {CTE_CREATE},
            {SharedSql.CASE_TYPE_CASE_PARTY_TYPE_DOCUMENT}
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
            		'VocabularyIdTagging',
                    (select id from tagging_vocabulary),
                    'Tags', 
                    null,
                    'TenantNodes',
                    null,
                    'Tenants',
                    (select document from tenants_document),
                    'Files',
                    null,
                    'Tags',
                    (select document from tags_document),
                    'CasePartyTypesCaseParties',
                    (select document from case_type_case_party_type_document)
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;
}
