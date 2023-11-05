namespace PoundPupLegacy.EditModel.Readers;

internal sealed class AbuseCaseUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<AbuseCase.ToUpdate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.ABUSE_CASE;

    const string SQL = $"""
            {SharedSql.CASE_UPDATE_CTE},
            {SharedSql.FAMILY_SIZES_DOCUMENT},
            {SharedSql.CHILD_PLACEMENT_TYPES_DOCUMENT},
            {SharedSql.TYPES_OF_ABUSER_FOR_UPDATE_DOCUMENT},
            {SharedSql.TYPES_OF_ABUSE_FOR_UPDATE_DOCUMENT}
            select
                jsonb_build_object(
                    'NodeIdentification',
                    (select document from identification_for_update_document where id = n.id),
                    'NodeDetailsForUpdate',
                    (select document from node_details_for_update_document where id = n.id),
                    'NameableDetails',
                    (select document from nameable_details_document where id = n.id),
                    'LocatableDetailsForUpdate',
                    (select document from locatable_details_document where id = n.id),
                    'CaseDetails',
                    (select document from case_details_document where id = n.id),
                    'AbuseCaseDetails',
                    jsonb_build_object(
                        'ChildPlacementTypeId',
                        ac.child_placement_type_id,
                        'FamilySizeId',
                        ac.family_size_id,
                        'HomeschoolingInvolved',
                        ac.home_schooling_involved,
                        'FundamentalFaithInvolved',
                        ac.fundamental_faith_involved,
                        'DisabilitiesInvolved',
                        ac.disabilities_involved,
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
            from node n
            join node_type nt on nt.id = n.node_type_id
            join nameable nm on nm.id = n.id
            join "case" c on c.id = n.id
            join abuse_case ac on ac.id = c.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.node_id = @node_id
        """;

    const string TYPE_OF_ABUSE_DOCUMENT = """
        type_of_abuse_document as(
            select
                jsonb_agg(
                    json_build_object(
                        'Id',
                        toa.id,
                        'Name',
                        toa.title,
                        'HasBeenDeleted',
                        false
                    )
                ) document
            from abuse_case_type_of_abuse actoa
            join node toa on toa.id = actoa.type_of_abuse_id
            join tenant_node tn on tn.node_id = actoa.abuse_case_id
            where tn.tenant_id = @tenant_id and tn.node_id = @node_id
        )
        """;
    const string TYPE_OF_ABUSER_DOCUMENT = """
        type_of_abuser_document as(
            select
                jsonb_agg(
                    json_build_object(
                        'Id',
                        toa.id,
                        'Name',
                        toa.title,
                        'HasBeenDeleted',
                        false
                    )
                ) document
            from abuse_case_type_of_abuser actoa
            join node toa on toa.id = actoa.type_of_abuser_id
            join tenant_node tn on tn.node_id = actoa.abuse_case_id
            where tn.tenant_id = @tenant_id and tn.node_id = @node_id
        )
        """;

}
