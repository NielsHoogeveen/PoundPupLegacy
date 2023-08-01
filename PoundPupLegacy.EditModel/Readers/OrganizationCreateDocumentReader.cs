namespace PoundPupLegacy.EditModel.Readers;

internal sealed class OrganizationCreateDocumentReaderFactory : NodeCreateDocumentReaderFactory<Organization.ToCreate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.ORGANIZATION;

    private const string SQL = $"""
            {SharedSql.PARTY_CREATE_CTE},
            {SharedSql.ORGANIZATION_TYPES_DOCUMENT},
            {SharedSql.INTER_ORGANIZATIONAL_RELATION_TYPES_DOCUMENT},
            {SharedSql.PERSON_ORGANIZATION_RELATION_TYPES_DOCUMENT},
            {SharedSql.ORGANIZATION_POLITICAL_ENTITY_RELATION_TYPES_DOCUMENT}
            select
                jsonb_build_object(
                    'NodeDetailsForCreate',
                    (select document from node_details_for_create_document),
                    'NameableDetails',
                    (select document from nameable_details_document),
                    'LocatableDetailsForCreate',
                    (select document from locatable_details_document),
                    'OrganizationDetailsForCreate',
                    jsonb_build_object(
                        'OrganizationTypes',
                        (select document from organization_types_document),
                        'OrganizationPoliticalEntityRelationTypes',
                        (select document from organization_political_entity_relation_types_document),
                        'InterOrganizationalRelationTypes',
                        (select document from inter_organizational_relation_types_document),
                        'PersonOrganizationRelationTypes',
                        (select document from person_organization_relation_types_document),
                        'OrganizationOrganizationTypes',
                        null,
                        'WebSiteUrl', 
                        null,
                        'EmailAddress',
                        null,
                        'Establishment', 
                        null,
                        'Termination',
                        null,
                        'OrganizationName',
                        jsonb_build_object(
                            'Name',
                            ''
                        )
                    )
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;


}
