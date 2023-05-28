namespace PoundPupLegacy.EditModel.Readers;

internal sealed class OrganizationCreateDocumentReaderFactory : NodeCreateDocumentReaderFactory<Organization.NewOrganization>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.ORGANIZATION;

    private const string SQL = $"""
            {CTE_CREATE},
            {SharedSql.ORGANIZATION_TYPES_DOCUMENT},
            {SharedSql.INTER_ORGANIZATIONAL_RELATION_TYPES_DOCUMENT},
            {SharedSql.PERSON_ORGANIZATION_RELATION_TYPES_DOCUMENT},
            {SharedSql.ORGANIZATION_POLITICAL_ENTITY_RELATION_TYPES_DOCUMENT}
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
                    'VocabularyIdTagging',
                    (select id from tagging_vocabulary),
                    'Tags',
                    (select document from tags_document),
                    'OrganizationTypes',
                    (select document from organization_types_document),
                    'PartyPoliticalEntityRelationTypes',
                    (select document from organization_political_entity_relation_types_document),
                    'InterOrganizationalRelationTypes',
                    (select document from inter_organizational_relation_types_document),
                    'PersonOrganizationRelationTypes',
                    (select document from person_organization_relation_types_document)
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;


}
