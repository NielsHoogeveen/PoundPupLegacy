namespace PoundPupLegacy.Convert;

internal sealed class ReviewMigrator(
        IDatabaseConnections databaseConnections,
        IEntityCreatorFactory<EventuallyIdentifiableBlogPost> blogPostCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "reviews";

    protected override async Task MigrateImpl()
    {
        await using var blogPostCreator = await blogPostCreatorFactory.CreateAsync(_postgresConnection);
        await blogPostCreator.CreateAsync(ReadReviews());
    }
    private async IAsyncEnumerable<NewBlogPost> ReadReviews()
    {

        var sql = $"""
                SELECT
                     n.nid id,
                     n.uid user_id,
                     n.title,
                     n.`status`,
                     FROM_UNIXTIME(n.created) created, 
                     FROM_UNIXTIME(n.changed) `changed`,
                     nr.body `text`
                FROM node n
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                WHERE n.`type` = 'review' AND n.uid <> 0
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();


        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            yield return new NewBlogPost {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                OwnerId = Constants.PPL,
                AuthoringStatusId = 1,
                TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("status"),
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },

                NodeTypeId = 35,
                Text = TextToHtml(reader.GetString("text")),
                Teaser = TextToTeaser(reader.GetString("text")),
                TermIds = new List<int>(),
            };

        }
        await reader.CloseAsync();
    }

}
