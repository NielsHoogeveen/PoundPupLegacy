namespace PoundPupLegacy.Convert;

internal sealed class AbuseCaseTypeOfAbuserMigrator(
    IDatabaseConnections databaseConnections,
    IEntityCreatorFactory<AbuseCaseTypeOfAbuser> abuseCaseTypeOfAbuserCreatorFactory,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "abuse case types of abuser";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var abuseCaseTypeOfAbuserCreator = await abuseCaseTypeOfAbuserCreatorFactory.CreateAsync(_postgresConnection);
        await abuseCaseTypeOfAbuserCreator.CreateAsync(ReadAbuseCaseTypeOfAbusers(nodeIdReader));
    }
    private async IAsyncEnumerable<AbuseCaseTypeOfAbuser> ReadAbuseCaseTypeOfAbusers(IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
            SELECT
            n.nid url_id,
            case
                when cfa.field_abuser_value = 'Adoptive father' then 131
                when cfa.field_abuser_value = 'Adoptive mother' then 7990
                when cfa.field_abuser_value = 'Adopted sibling' then 136
                when cfa.field_abuser_value = 'Foster father' then 132
                when cfa.field_abuser_value = 'Foster mother' then 134
                when cfa.field_abuser_value = 'Foster sibling' then 137
                when cfa.field_abuser_value = 'Non-fostered sibling' then 139
                when cfa.field_abuser_value = 'Non-adopted sibling' then 138
                when cfa.field_abuser_value = 'Other family member' then 140
                when cfa.field_abuser_value = 'Other non-family member' then 141
                when cfa.field_abuser_value = 'Legal guardian' then 40242
                when cfa.field_abuser_value = 'Undetermined' then 142
                ELSE null
            END type_of_abuser_id	
            FROM content_field_abuser cfa
            JOIN node n ON n.nid = cfa.nid AND n.vid = cfa.vid
            ORDER BY field_abuser_value
            """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {

            yield return new AbuseCaseTypeOfAbuser {
                AbuseCaseId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = 1,
                    UrlId = reader.GetInt32(0)
                }),
                TypeOfAbuserId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = 1,
                    UrlId = reader.GetInt32(1)
                }),
            };
        }
        await reader.CloseAsync();
    }
}
