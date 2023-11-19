namespace PoundPupLegacy.EditModel.Readers;

internal sealed class UnitedStatesCityCreateDocumentReaderFactory : NodeCreateDocumentReaderFactory<UnitedStatesCity.ToCreate>
{
    public override string Sql => SQL;

    protected override int NodeTypeId => Constants.UNITED_STATES_CITY;

    private const string SQL = $"""
            {SharedSql.NAMEABLE_CREATE_CTE}
            select
                jsonb_build_object(
                    'NodeDetailsForCreate',
                    (select document from node_details_for_create_document),
                    'NameableDetails',
                    (select document from nameable_details_document),
                    'Population',
                    0,
                    'Density',
                    0,
                    'Military',
                    false,
                    'Incorporated',
                    false,
                    'Latitude',
                    0,
                    'Longitude',
                    0,
                    'Timezone',
                    'America/Adak',
                    'CountyId',
                    0,
                    'CountyName',
                    '',
                    'SimpleName',
                    ''
                ) document
                from node_type nt
                where nt.id = @node_type_id
        """;
}
