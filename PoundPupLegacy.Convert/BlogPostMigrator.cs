using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class BlogPostMigrator: Migrator
{
    public BlogPostMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "blog posts";

    protected override async Task MigrateImpl()
    {
        await BlogPostCreator.CreateAsync(ReadBlogPosts(), _postgresConnection);

    }
    private async IAsyncEnumerable<BlogPost> ReadBlogPosts()
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
                WHERE n.`type` = 'blog' AND n.uid <> 0
                """;
        using var readCommand = _mysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32("id");
            var text = ReplacePHPCode(id, reader.GetString("text"));
            var discussion = new BlogPost
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
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = 35,
                Text = TextToHtml(text),
                Teaser = TextToTeaser(text),


            };
            yield return discussion;

        }
        await reader.CloseAsync();
    }

    private static string ReplacePHPCode(int id, string text)
    {
        List<string> codeSections = id switch
        {
            38035 => new List<string> { "ExecutiveCompensations" },
            38888 => new List<string> { "CongressionalSupport" },
            41136 => new List<string> { "HagueFacilitation"},
            45514 => new List<string> { "AbuseTotal", "AbusePerCountry", "AbuseMap", "AbusePerState", },
            _ => new List<string>()
        };
        if(codeSections.Count == 0)
        {
            return text;
        }
        var end = 0;
        List<string> parts = new List<string>();
        foreach(var (codeSection, index) in codeSections.Select((c, i) => (c, i)))
        {
            var rem = text.Substring(end);
            var start = text.IndexOf("<?php", end);
            var p = text.Substring(end, start - end);
            parts.Add(p);
            parts.Add(@$"<snippet name=""{codeSection}"">");
            end = text.IndexOf("?>", start) + 2;
        }
        var part = text.Substring(end, text.Length - end);
        parts.Add(part);
        return parts.Aggregate("", (a, b) => a + b);
    }
}
