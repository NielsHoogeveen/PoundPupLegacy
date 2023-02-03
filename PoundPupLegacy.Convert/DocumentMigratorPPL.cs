using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class DocumentMigratorPPL: PPLMigrator
{
    public DocumentMigratorPPL(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "documents ppl";

    protected override async Task MigrateImpl()
    {
        await DocumentCreator.CreateAsync(ReadDocuments(), _postgresConnection);
    }

    private async IAsyncEnumerable<Document> ReadDocuments()
    {

        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    10 node_type_id,
                    r.field_report_date_value publication_date,
                    case when r.field_web_address_url = '' then null else r.field_web_address_url end source_url,
                    nr.body `text`,
                    c.document_type_id
                FROM node n
                JOIN content_type_adopt_ind_rep r ON r.nid = n.nid AND r.vid = n.vid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                LEFT JOIN (
                select 
                    c.nid,
                    n2.nid document_type_id
                FROM category_node c 
                JOIN node n2 ON n2.nid = c.cid
                JOIN category cat ON cat.cid = c.cid AND cat.cnid = 42416 
                JOIN node n3 ON n3.nid = cat.cnid 
                ) c ON c.nid = n.nid 
                WHERE n.`type` = 'adopt_ind_rep'
                UNION
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    10 node_type_id,
                    r.field_date_value publication_date,
                    case when r.field_source_url = '' then null else r.field_source_url end source_url,
                    r.field_body_value `text`,
                    c.document_type_id
                FROM node n
                JOIN content_type_case_file r ON r.nid = n.nid AND r.vid = n.vid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                LEFT JOIN (
                SELECT
                    c.nid,
                    n2.nid document_type_id
                FROM 
                category_node c 
                JOIN node n2 ON n2.nid = c.cid
                JOIN category cat ON cat.cid = c.cid AND cat.cnid = 42416 
                ) c ON c.nid = n.nid 
                WHERE n.`type` = 'case_file'
                """;
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var publicationDate = StringToDateTimeRange(reader.IsDBNull("publication_date") ? null : reader.GetString("publication_date"));
            var id = reader.GetInt32("id");
            yield return new Document
            {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                OwnerId = Constants.OWNER_DOCUMENTATION,
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
                    },
                    new TenantNode
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
                NodeTypeId = reader.GetInt16("node_type_id"),
                PublicationDate = publicationDate,
                SourceUrl = reader.IsDBNull("source_url") ? null : reader.GetString("source_url"),
                Text = TextToHtml(reader.GetString("text")),
                Teaser = TextToTeaser(reader.GetString("text")),
                DocumentTypeId = reader.IsDBNull("document_type_id") ? null : await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("document_type_id")),
                Documentables = new List<int>(),
            };

        }
        await reader.CloseAsync();
    }

}
