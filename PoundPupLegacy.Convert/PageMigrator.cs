using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class PageMigrator : PPLMigrator
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
                     nr.body `text`,
                     ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                WHERE n.`type` = 'page' AND n.uid <> 0 and n.nid not in (63169)
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
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("status"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },

                NodeTypeId = 42,
                Text = TextToHtml(reader.GetString("text")),
                Teaser = TextToTeaser(reader.GetString("text")),
            };
        }
        await reader.CloseAsync();
    }

}
