namespace PoundPupLegacy.Convert;

internal sealed class DiscussionMigrator(
    IDatabaseConnections databaseConnections,
    IEntityCreatorFactory<Discussion.ToCreate> discussionCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "discussions";

    protected override async Task MigrateImpl()
    {
        await using var discussionCreator = await discussionCreatorFactory.CreateAsync(_postgresConnection);
        await discussionCreator.CreateAsync(ReadDiscussions());
    }
    private async IAsyncEnumerable<Discussion.ToCreate> ReadDiscussions()
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
                WHERE n.`type` = 'discussion' AND n.uid not in (0, 39)
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var discussion = new Discussion.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.NodeDetailsForCreate {
                    PublisherId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created"),
                    ChangedDateTime = reader.GetDateTime("changed"),
                    Title = reader.GetString("title"),
                    OwnerId = Constants.PPL,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreateForNewNode>
                    {
                        new TenantNode.ToCreateForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = 1,
                            PublicationStatusId = reader.GetInt32("status"),
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = id
                        }
                    },
                    NodeTypeId = 37,
                    TermIds = new List<int>(),
                },
                SimpleTextNodeDetails = new SimpleTextNodeDetails {
                    Text = TextToHtml(reader.GetString("text")),
                    Teaser = TextToTeaser(reader.GetString("text")),
                }
            };
            yield return discussion;
        }
        await reader.CloseAsync();
    }
}
