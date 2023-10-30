namespace PoundPupLegacy.EditModel.Readers;

internal sealed class PartyPoliticalEntityRelationTypeUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<PartyPoliticalEntityRelationType.ToUpdate>
{
    public override string Sql => string.Format(SQL, "tn");

    protected override int NodeTypeId => Constants.PARTY_POLITICAL_ENTITY_RELATION_TYPE;

    const string SQL = $"""
            {SharedSql.NAMEABLE_UPDATE_CTE}
                select
                jsonb_build_object(
                    'NodeIdentification',
                    (select document from identification_for_update_document where id = n.id),
                    'NodeDetailsForUpdate',
                    (select document from node_details_for_update_document where id = n.id),
                    'NameableDetails',
                    (select document from nameable_details_document where id = n.id)
                ) document
            from node n
            join node_type nt on nt.id = n.node_type_id
            join nameable nm on nm.id = n.id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id and n.node_type_id = @node_type_id
        """;
}
