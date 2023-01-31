using PoundPupLegacy.Db;
using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;
using System.Data;
using System.Reflection.PortableExecutable;

namespace PoundPupLegacy.Convert;

internal sealed class InformalIntermediateLevelSubdivisionMigrator : Migrator
{
    protected override string Name => "informal intermediate level subdivisions";

    public InformalIntermediateLevelSubdivisionMigrator(MySqlToPostgresConverter converter) : base(converter) { }

    private async IAsyncEnumerable<InformalIntermediateLevelSubdivision> ReadInformalIntermediateLevelSubdivisionCsv()
    {
        await using var vocabularyReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(_postgresConnection);
        await using var termReader = await TermReaderByName.CreateAsync(_postgresConnection);

        var vocabularyId = await vocabularyReader.ReadAsync(Constants.OWNER_GEOGRAPHY, "Subdivision type");

        await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\files\InformalIntermediateLevelSubdivisions.csv").Skip(1))
        {

            var parts = line.Split(new char[] { ';' }).Select(x => x.TrimStart()).ToList();
            int? id = int.Parse(parts[0]) == 0 ? null: int.Parse(parts[0]);
            var title = parts[8];
            var countryId = await _nodeIdReader.ReadAsync(Constants.PPL, int.Parse(parts[7]));
            var countryName = (await _termReaderByNameableId.ReadAsync(Constants.PPL, Constants.VOCABULARY_TOPICS, countryId)).Name;
            yield return new InformalIntermediateLevelSubdivision
            {
                Id = null,
                CreatedDateTime = DateTime.Parse(parts[1]),
                ChangedDateTime = DateTime.Parse(parts[2]),
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = title,
                        ParentNames = new List<string> { countryName },
                    }
                },
                Description = "",
                FileIdTileImage = null,
                NodeTypeId = int.Parse(parts[4]),
                OwnerId = Constants.OWNER_GEOGRAPHY,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
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
                SubdivisionTypeId = (await termReader.ReadAsync(vocabularyId, "Region")).NameableId
            };
        }
    }

    protected override async Task MigrateImpl()
    {
        await InformalIntermediateLevelSubdivisionCreator.CreateAsync(ReadInformalIntermediateLevelSubdivisionCsv(), _postgresConnection);
        await InformalIntermediateLevelSubdivisionCreator.CreateAsync(ReadInformalIntermediateLevelSubdivisions(), _postgresConnection);
    }
    private async IAsyncEnumerable<InformalIntermediateLevelSubdivision> ReadInformalIntermediateLevelSubdivisions()
    {
        await using var vocabularyReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(_postgresConnection);
        await using var termReader = await TermReaderByName.CreateAsync(_postgresConnection);

        var vocabularyId = await vocabularyReader.ReadAsync(Constants.OWNER_GEOGRAPHY, "Subdivision type");

        var sql = $"""
            SELECT
                n.nid id,
                n.uid access_role_id,
                n.title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                n2.nid country_id,
                ua.dst url_path
            FROM node n 
            LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
            JOIN category_hierarchy ch ON ch.cid = n.nid
            JOIN node n2 ON n2.nid = ch.parent
            WHERE n.`type` = 'region_facts'AND n2.`type` = 'country_type'
            """;
        using var readCommand = _mysqlConnectionPPL.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {

            var id = reader.GetInt32("id");
            var title = $"{reader.GetString("title")} (region of the USA)";
            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.PPL,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = title,
                    ParentNames = new List<string>{ "United States of America" },
                }
            };

            yield return new InformalIntermediateLevelSubdivision
            {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = title,
                OwnerId = Constants.OWNER_GEOGRAPHY,
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
                NodeTypeId = 18,
                CountryId = await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("country_id")),
                Name = reader.GetString("title"),
                VocabularyNames = vocabularyNames,
                Description = "",
                FileIdTileImage = null,
                SubdivisionTypeId = (await termReader.ReadAsync(vocabularyId, "Region")).NameableId
            };
        }
        await reader.CloseAsync();
    }
}
