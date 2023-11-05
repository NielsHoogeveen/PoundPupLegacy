namespace PoundPupLegacy.EditModel.Readers;

internal sealed class ChildTraffickingCaseUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<UnresolvedChildTraffickingCase.ToUpdate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.CHILD_TRAFFICKING_CASE;

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
                    'ResolvedChildTraffickingCaseDetails',
                    jsonb_build_object(
                        'CountryFromExisting',
                        jsonb_build_object(
                            'Id',
                            n3.id,
                            'Name',
                            n3.title
                        ),
                        'NumberOfChildrenInvolved',
                        ctc.number_of_children_involved
                    )
                ) document
            from node n
            join nameable nm on nm.id = n.id
            join "case" c on c.id = n.id
            join child_trafficking_case ctc on ctc.id = c.id
            join node n3 on n3.id = ctc.country_id_from
            join tenant_node tn on tn.node_id = n.id
            join node n2 on n2.id = ctc.country_id_from
            where tn.tenant_id = @tenant_id and tn.node_id = @node_id
        """;

}
