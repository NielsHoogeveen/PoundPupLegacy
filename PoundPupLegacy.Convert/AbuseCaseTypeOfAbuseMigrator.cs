namespace PoundPupLegacy.Convert;

internal sealed class AbuseCaseTypeOfAbuseMigrator : MigratorPPL
{
    private readonly IEntityCreator<AbuseCaseTypeOfAbuse> _abuseCaseTypeOfAbuseCreator;
    private readonly IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> _nodeIdReaderFactory;

    public AbuseCaseTypeOfAbuseMigrator(
        IDatabaseConnections databaseConnections,
        IEntityCreator<AbuseCaseTypeOfAbuse> abuseCaseTypeOfAbuseCreator,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory
    ) : base(databaseConnections)
    {
        _abuseCaseTypeOfAbuseCreator = abuseCaseTypeOfAbuseCreator;
        _nodeIdReaderFactory = nodeIdReaderFactory;
    }

    protected override string Name => "abuse case types of abuse";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await _nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await _abuseCaseTypeOfAbuseCreator.CreateAsync(ReadAbuseCaseTypeOfAbuses(nodeIdReader), _postgresConnection);
    }
    private async IAsyncEnumerable<AbuseCaseTypeOfAbuse> ReadAbuseCaseTypeOfAbuses(IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
            SELECT
            n.nid url_id,
            case
            	when cfa.field_type_of_abuse_value = 'Death by unknown cause' then 130
            	when cfa.field_type_of_abuse_value = 'Economic exploitation' then 44447
            	when cfa.field_type_of_abuse_value = 'Lethal deprivation' then 74533
            	when cfa.field_type_of_abuse_value = 'Lethal neglect' then 74428
            	when cfa.field_type_of_abuse_value = 'Lethal physical abuse' then 119
            	when cfa.field_type_of_abuse_value = 'Medical abuse' then 73882
            	when cfa.field_type_of_abuse_value = 'Non-lethal deprivation' then 125
            	when cfa.field_type_of_abuse_value = 'Non-lethal neglect' then 123
            	when cfa.field_type_of_abuse_value = 'Non-lethal physical abuse' then 118
            	when cfa.field_type_of_abuse_value = 'Sexual abuse' then 6979
            	when cfa.field_type_of_abuse_value = 'Sexual exploitation' then 23662
            	when cfa.field_type_of_abuse_value = 'Verbal abuse' then 74006
            	ELSE null
            END type_of_abuse_id	
            FROM content_field_type_of_abuse cfa
            JOIN node n ON n.nid = cfa.nid AND n.vid = cfa.vid
            ORDER BY field_type_of_abuse_value
            """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {

            yield return new AbuseCaseTypeOfAbuse { 
                AbuseCaseId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest 
                { 
                    TenantId = 1,
                    UrlId = reader.GetInt32(0) 
                }),
                TypeOfAbuseId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest 
                {
                    TenantId = 1,
                    UrlId = reader.GetInt32(1) 
                }),
            };
        }
        await reader.CloseAsync();
    }
}
