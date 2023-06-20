namespace PoundPupLegacy.EditModel.Readers;

internal sealed class DisruptedPlacementCaseUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<DisruptedPlacementCase.ToUpdate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.DISRUPTED_PLACEMENT_CASE;

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
                    (select document from case_details_document where id = n.id)
                ) document
            from node n
            join nameable nm on nm.id = n.id
            join "case" c on c.id = n.id
            join disrupted_placement_case dpc on dpc.id = c.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        """;

}
