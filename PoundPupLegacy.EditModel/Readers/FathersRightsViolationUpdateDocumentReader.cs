namespace PoundPupLegacy.EditModel.Readers;

internal sealed class FathersRightsViolationCaseUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<FathersRightsViolationCase.ToUpdate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.FATHERS_RIGHTS_VIOLATION_CASE;

    const string SQL = $"""
            {SharedSql.CASE_UPDATE_CTE}
            select
                jsonb_build_object(
                    'NodeIdentification',
                    (select document from identification_document where id = n.id),
                    'NodeDetailsForUpdate',
                    (select document from node_details_document where id = n.id),
                    'NamebleDetails',
                    (select document from nameable_details_document where id = n.id),
                    'LocationDetails',
                    (select document from locatable_details_document where id = n.id),
                    'CaseDetails',
                    (select document from case_details_document where id = n.id)
                ) document
            from node n
            join nameable nm on nm.id = n.id
            join "case" c on c.id = n.id
            join fathers_rights_violation_case frc on frc.id = c.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        """;

}
