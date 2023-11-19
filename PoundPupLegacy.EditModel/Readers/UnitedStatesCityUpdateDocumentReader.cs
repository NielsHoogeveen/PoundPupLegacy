namespace PoundPupLegacy.EditModel.Readers;

internal sealed class UnitedStatesCityUpdateDocumentReaderFactory : NodeUpdateDocumentReaderFactory<UnitedStatesCity.ToUpdate>
{
    public override string Sql => string.Format(SQL, "tn");

    protected override int NodeTypeId => Constants.UNITED_STATES_CITY;

    const string SQL = $"""
            {SharedSql.NAMEABLE_UPDATE_CTE}
                select
                jsonb_build_object(
                    'NodeIdentification',
                    (select document from identification_for_update_document where id = n.id),
                    'NodeDetailsForUpdate',
                    (select document from node_details_for_update_document where id = n.id),
                    'NameableDetails',
                    (select document from nameable_details_document where id = n.id),
                    'Population',
                    usc.population,
                    'Density',
                    usc.density,
                    'Military',
                    usc.military,
                    'Incorporated',
                    usc.incorporated,
                    'Latitude',
                    usc.latitude,
                    'Longitude',
                    usc.longitude,
                    'Timezone',
                    usc.timezone,
                    'CountyId',
                    usc.county_id,
                    'CountyName',
                    n2.title,
                    'SimpleName',
                    usc.simple_name
                ) document
            from node n
            join node_type nt on nt.id = n.node_type_id
            join nameable nm on nm.id = n.id
            join united_states_city usc on usc.id = n.id
            join node n2 on n2.id = usc.county_id
            join tenant_node tn on tn.node_id = n.id
            where tn.tenant_id = @tenant_id and tn.node_id = @node_id 
        """;
}
