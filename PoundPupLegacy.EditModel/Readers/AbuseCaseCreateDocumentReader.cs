namespace PoundPupLegacy.EditModel.Readers;

internal sealed class AbuseCaseCreateDocumentReaderFactory : NodeCreateDocumentReaderFactory<AbuseCase.ToCreate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.ABUSE_CASE;

    private const string SQL = $"""
            {SharedSql.CASE_CREATE_CTE},
            {SharedSql.FAMILY_SIZES_DOCUMENT},
            {SharedSql.CHILD_PLACEMENT_TYPES_DOCUMENT},
            {SharedSql.TYPES_OF_ABUSER_FOR_CREATE_DOCUMENT},
            {SharedSql.TYPES_OF_ABUSE_FOR_CREATE_DOCUMENT}
            select
                jsonb_build_object(
                    'NodeDetailsForCreate',
                    (select document from node_details_for_create_document),
                    'NameableDetails',
                    (select document from nameable_details_document),
                    'LocatableDetailsForCreate',
                    (select document from locatable_details_document),
                    'CaseDetails',
                    (select document from case_details_document),
                    'AbuseCaseDetails',
                    jsonb_build_object(
                        'ChildPlacementTypeId',
                        101250,
                        'FamilySizeId',
                        null,
                        'HomeschoolingInvolved',
                        null,
                        'FundamentalFaithInvolved',
                        null,
                        'DisabilitiesInvolved',
                        null,
                        'FamilySizesToSelectFrom',
                        (select document from family_sizes_document),
                        'ChildPlacementTypesToSelectFrom',
                        (select document from child_placement_types_document),
                        'TypesOfAbuse',
                        (select document from types_of_abuse_document),
                        'TypesOfAbuser',
                        (select document from types_of_abuser_document)
                    )
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;
}
