using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class BasicSecondLevelSubdivisionMigrator : Migrator
{
    protected override string Name => "basic second level subdivisions";

    public BasicSecondLevelSubdivisionMigrator(MySqlToPostgresConverter converter): base(converter) { } 

    private async IAsyncEnumerable<BasicSecondLevelSubdivision> ReadBasicSecondLevelSubdivisionsInInformalPrimarySubdivisionCsv()
    {
        await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\files\BasicSecondLevelSubdivisionsInInformalPrimarySubdivision.csv").Skip(1))
        {
            var parts = line.Split(new char[] { ';' });
            int? id = int.Parse(parts[0]) == 0? null: int.Parse(parts[0]);
            var title = parts[8];
            var countryId = await _nodeIdReader.ReadAsync(Constants.PPL, int.Parse(parts[7]));
            var subdivisionId = await _subdivisionIdReader.ReadAsync(countryId, parts[11]);
            var topicName = (await _termReaderByNameableId.ReadAsync(Constants.PPL, Constants.VOCABULARY_TOPICS, subdivisionId)).Name;
            yield return new BasicSecondLevelSubdivision
            {
                Id = null,
                CreatedDateTime = DateTime.Parse(parts[1]),
                ChangedDateTime = DateTime.Parse(parts[2]),
                NodeTypeId = int.Parse(parts[4]),
                OwnerId = Constants.OWNER_GEOGRAPHY,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = int.Parse(parts[5]),
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                PublisherId = int.Parse(parts[6]),
                CountryId = countryId,
                Title = title,
                Name = parts[9],
                Description = "",
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = title,
                        ParentNames = new List<string> { topicName },
                    }
                },
                ISO3166_2_Code = parts[10],
                IntermediateLevelSubdivisionId = subdivisionId,
                FileIdFlag = null,
                FileIdTileImage = null,
            };
        }
    }

    private async IAsyncEnumerable<BasicSecondLevelSubdivision> ReadBasicSecondLevelSubdivisionCsv()
    {
        await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\files\BasicSecondLevelSubdivisions.csv").Skip(1))
        {

            var parts = line.Split(new char[] { ';' });
            int? id = int.Parse(parts[0]) == 0 ? null : int.Parse(parts[0]);
            var title = parts[8];
            var subdivisionId = await _subdivisionIdReaderByIso3166Code.ReadAsync(parts[11]);
            var topicName = (await _termReaderByNameableId.ReadAsync(Constants.PPL, Constants.VOCABULARY_TOPICS, subdivisionId)).Name;
            yield return new BasicSecondLevelSubdivision
            {
                Id = null,
                CreatedDateTime = DateTime.Parse(parts[1]),
                ChangedDateTime = DateTime.Parse(parts[2]),
                NodeTypeId = int.Parse(parts[4]),
                OwnerId = Constants.OWNER_GEOGRAPHY,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = int.Parse(parts[5]),
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                PublisherId = int.Parse(parts[6]),
                CountryId = await _nodeIdReader.ReadAsync(Constants.PPL, int.Parse(parts[7])),
                Title = title,
                Name = parts[9],
                Description = "",
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = title,
                        ParentNames = new List<string> { topicName },
                    }
                },
                ISO3166_2_Code = parts[10],
                IntermediateLevelSubdivisionId = subdivisionId,
                FileIdFlag = null,
                FileIdTileImage = null,
            };
        }
    }
    protected override async Task MigrateImpl()
    {
        await BasicSecondLevelSubdivisionCreator.CreateAsync(ReadBasicSecondLevelSubdivisionsInInformalPrimarySubdivisionCsv(), _postgresConnection);
        await BasicSecondLevelSubdivisionCreator.CreateAsync(ReadBasicSecondLevelSubdivisionCsv(), _postgresConnection);
        await BasicSecondLevelSubdivisionCreator.CreateAsync(ReadBasicSecondLevelSubdivisions(), _postgresConnection);

    }
    private async IAsyncEnumerable<BasicSecondLevelSubdivision> ReadBasicSecondLevelSubdivisions()
    {
        var sql = $"""
            SELECT
                n.nid id,
                n.uid access_role_id,
                n.title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                n2.nid intermediate_level_subdivision_id,
                n2.title subdivision_name,
                3805 country_id,
                CONCAT('US-', s.field_statecode_value) 
                iso_3166_2_code,
                s.field_state_flag_fid file_id_flag,
                ua.dst url_path
            FROM node n 
            LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
            JOIN content_type_statefact s ON s.nid = n.nid
            JOIN category_hierarchy ch ON ch.cid = n.nid
            JOIN node n2 ON n2.nid = ch.parent
            WHERE n.`type` = 'statefact'
            AND n2.`type` = 'region_facts'
            AND s.field_statecode_value IS NOT NULL
            ORDER BY s.field_statecode_value
            """;
        using var readCommand = _mysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32("id");
            var title = $"{reader.GetString("title").Replace(" (state)", "")} (state of the USA)";
            var subdivisioName = $"{reader.GetString("subdivision_name")} (region of the USA)";
            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.PPL, 
                    Name = Constants.VOCABULARY_TOPICS, 
                    TermName = title,
                    ParentNames = new List<string>{ subdivisioName },
                }
            };
            yield return new BasicSecondLevelSubdivision
            {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = title,
                Name = reader.GetString("title"),
                OwnerId = Constants.OWNER_GEOGRAPHY,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = reader.GetInt32("id")
                    }
                },
                NodeTypeId = 19,
                Description = "",
                VocabularyNames = vocabularyNames,
                IntermediateLevelSubdivisionId = await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("intermediate_level_subdivision_id")),
                CountryId = await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("country_id")),
                ISO3166_2_Code = reader.GetString("iso_3166_2_code"),
                FileIdFlag = reader.IsDBNull("file_id_flag") ? null : reader.GetInt32("file_id_flag"),
                FileIdTileImage = null,
            };
        }
        await reader.CloseAsync();
    }

}
