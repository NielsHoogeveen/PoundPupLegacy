using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class BasicNameableMigrator : PPLMigrator
{
    public BasicNameableMigrator(MySqlToPostgresConverter converter) : base(converter) { }

    protected override string Name => "basic nameables";

    protected override async Task MigrateImpl()
    {
        await BasicNameableCreator.CreateAsync(ReadBasicNameables(), _postgresConnection);
    }
    private async IAsyncEnumerable<BasicNameable> ReadBasicNameables()
    {

        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    41 node_type_id,
                    nr.body description,
                    n2.`type`,
                case 
                    when field_tile_image_fid = 0 then null
                    ELSE field_tile_image_fid
                END file_id_tile_image,
                ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN category c ON c.cid = n.nid AND c.cnid = 4126
                JOIN content_type_category_cat cc ON cc.nid = n.nid AND cc.vid = n.vid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                LEFT JOIN node n2 ON n2.title = n.title AND n2.nid <> n.nid AND n2.`type` IN ('adopt_person','country_type', 'adopt_orgs', 'case', 'region_facts', 'coerced_adoption_cases', 'child_trafficking', 'child_trafficking_case')
                LEFT JOIN node n3 ON n3.nid = cc.field_related_page_nid
                WHERE  (n3.nid IS NULL OR n3.`type` = 'group')
                AND n.title NOT IN 
                (
                    'adoption',
                    'adoptive mother',
                    'foster care',
                    'guardianship',
                    'institutional care',
                    'adoption agencies',
                    'legal guardians',
                    'church',
                    'boot camp',
                    'media',
                    'blog',
                    'orphanages',
                    'orphanage',
                    'adoption advocates',
                    'boarding school',
                    'adoption facilitators',
                    'maternity homes',
                    'sexual abuse',
                    'sexual exploitation',
                    'lethal neglect',
                    'lethal deprivation',
                    'economic exploitation',
                    'verbal abuse',
                    'medical abuse',
                    'lawyers',
                    'therapists',
                    'Christianity',
                    'Northern Ireland',
                    'mega families',
                    'Mary Landrieu',
                    'Hilary Clinton',
                    'Michele Bachmann',
                    'Kevin and Kody Pribbernow'
                )
                AND n.nid NOT IN (
                    22589, 74730
                )
                AND n2.nid IS  NULL
                """;
        using var readCommand = MysqlConnection.CreateCommand();
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
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = name,
                        ParentNames = new List<string>(),
                    }
                };

            yield return new BasicNameable
            {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = name,
                OwnerId = 1,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("status"),
                        UrlPath = reader.IsDBNull("url_path") ? null: reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = reader.GetInt32("node_type_id"),
                Description = reader.GetString("description"),
                FileIdTileImage = null,
                VocabularyNames = vocabularyNames,
            };

        }
        await reader.CloseAsync();
    }
}
