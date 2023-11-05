namespace PoundPupLegacy.EditModel.Readers;

internal sealed class DeportationCaseUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<DeportationCase.ToUpdate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.DEPORTATION_CASE;

    const string SQL = $"""
            {SharedSql.CASE_UPDATE_CTE}
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
                    'DeportationCaseDetails',
                    jsonb_build_object(
                        'SubdivisionFrom',
                        case
                            when n2.id is null then null
                            else jsonb_build_object(
                                'Id', 
                                n2.id,
                                'Name', 
                                n2.title
                            )
                        end,
                        'CountryTo',
                        case
                            when n1.id is null then null
                            else jsonb_build_object(
                                'Id', 
                                n1.id,
                                'Name', 
                                n1.title
                            )
                        end
                    )
                ) document
            from node n
            join nameable nm on nm.id = n.id
            join "case" c on c.id = n.id
            join deportation_case dc on dc.id = c.id
            join tenant_node tn on tn.node_id = n.id
            left join node n1 on n1.id = dc.country_id_to
            left join node n2 on n2.id = dc.subdivision_id_from
            where tn.tenant_id = @tenant_id and tn.node_id = @node_id 
        """;

}
