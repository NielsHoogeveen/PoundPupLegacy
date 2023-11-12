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
                    'NodeDetailsForCreate',
                    (select document from node_details_for_create_document),
                    'NameableDetails',
                    (select document from nameable_details_document),
                    'LocatableDetailsForCreate',
                    (select document from locatable_details_document),
                    'PersonDetailsForCreate',
                    jsonb_build_object(
                        'InterPersonalRelationTypes',
                        (select document from inter_personal_relation_types_document),
                        'PersonOrganizationRelationTypes',
                        (select document from person_organization_relation_types_document),
                        'PersonPoliticalEntityRelationTypes',
                        (select document from person_political_entity_relation_types_document),
                        'Name',
                        '',
                        'DateOfBirth',
                        null,
                        'DateOfDeath',
                        null,
                        'FileIdPortrait',
                        null,
                        'FirstName',
                        null,
                        'FullName',
                        null,
                        'LastName',
                        null,
                        'MiddleName',
                        null,
                        'Suffix',
                        null
                   )
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;
}
