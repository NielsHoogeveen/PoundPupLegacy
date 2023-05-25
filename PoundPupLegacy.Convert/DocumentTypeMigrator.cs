namespace PoundPupLegacy.Convert;

internal sealed class DocumentTypeMigrator(
    IDatabaseConnections databaseConnections,
    IEntityCreatorFactory<EventuallyIdentifiableDocumentType> documentTypeCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "document types";

    protected override async Task MigrateImpl()
    {
        await using var documentTypeCreator = await documentTypeCreatorFactory.CreateAsync(_postgresConnection);
        await documentTypeCreator.CreateAsync(ReadSelectionOptions());
    }
    private async IAsyncEnumerable<NewDocumentType> ReadSelectionOptions()
    {

        var sql = $"""
                  SELECT 
                    n2.nid id,
                    n2.uid access_role_id,
                    n2.title,
                    n2.`status` node_status_id,
                    FROM_UNIXTIME(n2.created) created_date_time, 
                    FROM_UNIXTIME(n2.changed) changed_date_time,
                    ua.dst url_path
                    FROM node n1 
                    JOIN category c ON c.cnid = n1.nid
                    JOIN node n2 ON n2.nid = c.cid
                    LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n2.nid
                    WHERE n1.nid  = 42416
                  """;

        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var name = reader.GetString("title");
            var id = reader.GetInt32("id");
            yield return new NewDocumentType {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                OwnerId = Constants.OWNER_DOCUMENTATION,
                AuthoringStatusId = 1,
                TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    },
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id < 33163 ? id : null
                    }
                },
                NodeTypeId = 9,
                Description = "",
                FileIdTileImage = null,
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.OWNER_DOCUMENTATION,
                        Name = Constants.VOCABULARY_DOCUMENT_TYPE,
                        TermName = name,
                        ParentNames = new List<string>(),
                    },
                },
                NodeTermIds = new List<int>(),
            };
        }
        await reader.CloseAsync();
    }
}
