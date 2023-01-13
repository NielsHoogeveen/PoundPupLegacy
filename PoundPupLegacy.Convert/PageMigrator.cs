using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class PageMigrator : Migrator
{
    public PageMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "pages";

    protected override async Task MigrateImpl()
    {
        await PageCreator.CreateAsync(ReadPages(), _postgresConnection);

    }
    private async IAsyncEnumerable<Page> ReadPages()
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
                WHERE n.`type` = 'page' AND n.uid <> 0
                """;
        using var readCommand = _mysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        
        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32("id");
            yield return new Page
            {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                OwnerId = Constants.PPL,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("status"),
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },

                NodeTypeId = 42,
                Text = reader.GetString("text"),

            };
        }
        await reader.CloseAsync();
    }

}
