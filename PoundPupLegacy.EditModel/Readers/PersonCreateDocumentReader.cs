namespace PoundPupLegacy.EditModel.Readers;

internal sealed class PersonCreateDocumentReaderFactory : NodeCreateDocumentReaderFactory<Person.ToCreate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.PERSON;

    private const string SQL = $"""
            {SharedSql.PARTY_CREATE_CTE},
            {SharedSql.INTER_PERSONAL_RELATION_TYPES_DOCUMENT},
            {SharedSql.PERSON_PERSONAL_RELATION_TYPES_DOCUMENT},
            {SharedSql.PERSON_POLITICAL_ENTITY_RELATION_TYPES_DOCUMENT}
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
                    'InterPersonalRelationTypes',
                    (select document from inter_personal_relation_types_document),
                    'PersonOrganizationRelationTypes',
                    (select document from person_organization_relation_types_document),
                    'PartyPoliticalEntityRelationTypes',
                    (select document from person_political_entity_relation_types_document)
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;
}
