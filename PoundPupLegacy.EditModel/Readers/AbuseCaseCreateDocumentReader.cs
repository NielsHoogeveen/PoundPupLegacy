namespace PoundPupLegacy.EditModel.Readers;

internal sealed class AbuseCaseCreateDocumentReaderFactory : NodeCreateDocumentReaderFactory<AbuseCase.ToCreate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.ABUSE_CASE;

    private const string SQL = $"""
            {CTE_CREATE},
            {SharedSql.FAMILY_SIZES_DOCUMENT},
            {SharedSql.CHILD_PLACEMENT_TYPES_DOCUMENT},
            {SharedSql.TYPES_OF_ABUSER_DOCUMENT},
            {SharedSql.TYPES_OF_ABUSE_DOCUMENT},
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
            		'Tags', null,
                    'TenantNodes',
                    null,
                    'Tenants',
                    (select document from tenants_document),
                    'Files',
                    null,
                    'Tags',
                    (select document from tags_document),
                    'VocabularyIdTagging',
                    (select id from tagging_vocabulary),
                    'CasePartyTypesCaseParties',
                    (select document from case_type_case_party_type_document),
                    'FamilySizesToSelectFrom',
                    (select document from family_sizes_document),
                    'ChildPlacementTypesToSelectFrom',
                    (select document from child_placement_types_document),
                    'TypesOfAbuseToSelectFrom',
                    (select document from types_of_abuse_document),
                    'TypesOfAbuserToSelectFrom',
                    (select document from types_of_abuser_document),
                    'TypesOfAbuse',
                    null,
                    'TypesOfAbuser',
                    null
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;
}
