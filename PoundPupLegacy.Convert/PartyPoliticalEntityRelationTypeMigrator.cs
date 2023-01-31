using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class PartyPoliticalEntityRelationTypeMigrator: PPLMigrator
{
    public PartyPoliticalEntityRelationTypeMigrator(MySqlToPostgresConverter converter) : base(converter) { }

    protected override string Name => "political entity relation types";

    protected override async Task MigrateImpl()
    {
        await PartyPoliticalEntityRelationTypeCreator.CreateAsync(ReadPoliticalEntityRelationTypes(), _postgresConnection);
    }
    private async IAsyncEnumerable<PartyPoliticalEntityRelationType> ReadPoliticalEntityRelationTypes()
    {

        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid access_role_id,
                    n.title,
                    n.`status` node_status_id,
                    FROM_UNIXTIME(n.created) created_date_time, 
                    FROM_UNIXTIME(n.changed) changed_date_time,
                    '' description,
                    NULL file_id_tile_image,
                    case 
                        when n.nid IN (12662, 12660) then true
                        ELSE false
                    END has_concrete_subtype,
                    ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                JOIN category c ON c.cid = n.nid AND c.cnid = 12652
                """;

        using var readCommand = _mysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");

            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_PARTIES,
                    Name = Constants.VOCABULARY_POLITICAL_ENTITY_RELATION_TYPE,
                    TermName = name,
                    ParentNames = new List<string>(),
                }
            };

            yield return new PartyPoliticalEntityRelationType
            {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                OwnerId = Constants.OWNER_PARTIES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = 3,
                Description = reader.GetString("description"),
                FileIdTileImage = reader.IsDBNull("file_id_tile_image") ? null : reader.GetInt32("file_id_tile_image"),
                HasConcreteSubtype = reader.GetBoolean("has_concrete_subtype"),
                VocabularyNames = vocabularyNames,
            };

        }
        await reader.CloseAsync();
    }
}
