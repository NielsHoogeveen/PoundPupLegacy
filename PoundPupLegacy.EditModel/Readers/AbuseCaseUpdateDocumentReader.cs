namespace PoundPupLegacy.EditModel.Readers;

internal sealed class AbuseCaseUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<ExistingAbuseCase>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.ABUSE_CASE;

    const string SQL = $"""
            {CTE_EDIT},
            {TYPE_OF_ABUSE_IDS_DOCUMENT},
            {TYPE_OF_ABUSER_IDS_DOCUMENT},
            {SharedSql.FAMILY_SIZES_DOCUMENT},
            {SharedSql.CHILD_PLACEMENT_TYPES_DOCUMENT},
            {SharedSql.TYPES_OF_ABUSER_DOCUMENT},
            {SharedSql.TYPES_OF_ABUSE_DOCUMENT},
            {SharedSql.CASE_CASE_PARTY_DOCUMENT}
            select
                jsonb_build_object(
                    'NodeId', 
                    n.id,
                    'UrlId', 
                    tn.url_id,
                    'PublisherId', 
                    n.publisher_id,
                    'OwnerId', 
                    n.owner_id,
                    'Title' , 
                    n.title,
                    'Description', 
                    nm.description,
                    'Date',
                    c.fuzzy_date,
                    'NodeTypeName',
                    nt.name,
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
                    'Tags', 
                    (select document from tags_document),
                    'TenantNodes',
                    (select document from tenant_nodes_document),
                    'Tenants',
                    (select document from tenants_document),
                    'Files',
                    (select document from attachments_document),
                    'Locations',
                    (select document from locations_document),
                    'CasePartyTypesCaseParties',
                    (select document from case_case_party_document),
                    'FamilySizes',
                    (select document from family_sizes_document),
                    'ChildPlacementTypes',
                    (select document from child_placement_types_document),
                    'TypesOfAbuse',
                    (select document from types_of_abuse_document),
                    'TypesOfAbuser',
                    (select document from types_of_abuser_document),
                    'TypeOfAbuseIds',
                    (select document from type_of_abuse_ids_document),
                    'TypeOfAbuserIds',
                    (select document from type_of_abuser_ids_document)
            ) document
            from node n
            join node_type nt on nt.id = n.node_type_id
            join nameable nm on nm.id = n.id
            join "case" c on c.id = n.id
            join abuse_case ac on ac.id = c.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        """;

    const string TYPE_OF_ABUSE_IDS_DOCUMENT = """
        type_of_abuse_ids_document as(
            select
            jsonb_agg(type_of_abuse_id) document
            from abuse_case_type_of_abuse actoa
            join tenant_node tn on tn.node_id = actoa.abuse_case_id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        )
        """;
    const string TYPE_OF_ABUSER_IDS_DOCUMENT = """
        type_of_abuser_ids_document as(
            select
            jsonb_agg(type_of_abuser_id) document
            from abuse_case_type_of_abuser actoa
            join tenant_node tn on tn.node_id = actoa.abuse_case_id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        )
        """;

}
