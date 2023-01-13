using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class CountryAndFirstAndSecondLevelSubdivisionMigrator: CountryMigrator
{
    protected override string Name => "countries that are both first and second level subdivisions";

    public CountryAndFirstAndSecondLevelSubdivisionMigrator(MySqlToPostgresConverter converter) : base(converter) { }

    private async IAsyncEnumerable<CountryAndFirstAndSecondLevelSubdivision> GetRegionSubdivisionCountries()
    {
        yield return new CountryAndFirstAndSecondLevelSubdivision
        {
            Id = null,
            Title = "Saint Barthélemy",
            Name = "Saint Barthélemy",
            OwnerId = Constants.OWNER_GEOGRAPHY,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.SAINT_BARTH
                    }
                },
            NodeTypeId = 16,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            PublisherId = 1,
            Description = "",
            VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = "Saint Barthélemy",
                        ParentNames = new List<string>{ "Caribbean" },
                    }
                },
            SecondLevelRegionId = await _nodeIdReader.ReadAsync(Constants.PPL, 3809),
            CountryId = await _nodeIdReader.ReadAsync(Constants.PPL, 4018),
            ISO3166_1_Code = "BL",
            ISO3166_2_Code = "FR-BL",
            FileIdFlag = null,
            FileIdTileImage = null,
            HagueStatusId = await _nodeIdReader.ReadAsync(Constants.PPL, 41213),
            ResidencyRequirements = null,
            AgeRequirements = null,
            HealthRequirements = null,
            IncomeRequirements = null,
            MarriageRequirements = null,
            OtherRequirements = null,

        };
        yield return new CountryAndFirstAndSecondLevelSubdivision
        {
            Id = null,
            Title = "Saint Martin",
            Name = "Saint Martin",
            OwnerId = Constants.OWNER_GEOGRAPHY,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.SAINT_MARTIN
                    }
                },
            NodeTypeId = 16,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            PublisherId = 1,
            Description = "",
            VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = "Saint Martin",
                        ParentNames = new List<string>{ "Caribbean" },
                    }
                },
            SecondLevelRegionId = await _nodeIdReader.ReadAsync(Constants.PPL, 3809),
            CountryId = await _nodeIdReader.ReadAsync(Constants.PPL, 4018),
            ISO3166_1_Code = "MF",
            ISO3166_2_Code = "FR-MF",
            FileIdFlag = null,
            FileIdTileImage = null,
            HagueStatusId = await _nodeIdReader.ReadAsync(Constants.PPL, 41213),
            ResidencyRequirements = null,
            AgeRequirements = null,
            HealthRequirements = null,
            IncomeRequirements = null,
            MarriageRequirements = null,
            OtherRequirements = null,
        };
        yield return new CountryAndFirstAndSecondLevelSubdivision
        {
            Id = null,
            Title = "French Southern Territories",
            Name = "French Southern Territories",
            OwnerId = Constants.OWNER_GEOGRAPHY,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.FRENCH_SOUTHERN_TERRITORIES
                    }
                },
            NodeTypeId = 15,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            PublisherId = 1,
            Description = "",
            VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = "French Southern Territories",
                        ParentNames = new List<string>{ "Southern Africa" },
                    }
                },
            SecondLevelRegionId = await _nodeIdReader.ReadAsync(Constants.PPL, 3828),
            CountryId = await _nodeIdReader.ReadAsync(Constants.PPL, 4018),
            ISO3166_1_Code = "TF",
            ISO3166_2_Code = "FR-TF",
            FileIdFlag = null,
            FileIdTileImage = null,
            HagueStatusId = await _nodeIdReader.ReadAsync(Constants.PPL, 41213),
            ResidencyRequirements = null,
            AgeRequirements = null,
            HealthRequirements = null,
            IncomeRequirements = null,
            MarriageRequirements = null,
            OtherRequirements = null,
        };
    }

    protected override async Task MigrateImpl()
    {
        await CountryAndFirstAndSecondLevelSubdivisionCreator.CreateAsync(GetRegionSubdivisionCountries(), _postgresConnection);
        await CountryAndFirstAndSecondLevelSubdivisionCreator.CreateAsync(ReadCountryAndFirstAndSecondLevelSubdivision(), _postgresConnection);
    }
    private async IAsyncEnumerable<CountryAndFirstAndSecondLevelSubdivision> ReadCountryAndFirstAndSecondLevelSubdivision()
    {


        var sql = $"""
            SELECT
                n.nid id,
                n.uid access_role_id,
                n.title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                n2.nid second_level_region_id,
                n2.title second_level_region_name,
                upper(cou.field_country_code_value) iso_3166_code,
                ua.dst url_path
            FROM node n 
            LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
            JOIN content_type_country_type cou ON cou.nid = n.nid
            JOIN category_hierarchy ch ON ch.cid = n.nid
            JOIN node n2 ON n2.nid = ch.parent
            WHERE n.`type` = 'country_type'
            AND n2.`type` = 'region_facts'
            AND n.nid IN (
                3935,
                3903,
                3908,
                4044,
                4057,
                3887,
                3879,
                4063,
                3878)
            """;
        using var readCommand = _mysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();


        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32("id");
            var name = reader.GetInt32("id") == 3879 ? "Réunion" :
                        reader.GetString("title");
            var regionName = reader.GetString("second_level_region_name");
            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.PPL,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = name,
                    ParentNames = new List<string>{ regionName },
                }
            };


            yield return new CountryAndFirstAndSecondLevelSubdivision
            {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                Name = name,
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
                        UrlId = id
                    }
                },
                NodeTypeId = 16,
                Description = "",
                VocabularyNames = vocabularyNames,
                SecondLevelRegionId = await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("second_level_region_id")),
                ISO3166_1_Code = id == 3847 ? "NE" :
                                 id == 4010 ? "RS" :
                                 id == 4014 ? "XK" :
                                 reader.GetString("iso_3166_code"),
                ISO3166_2_Code = GetISO3166Code2ForCountry(id),
                CountryId = await _nodeIdReader.ReadAsync(Constants.PPL, GetSupervisingCountryId(id)),
                FileIdFlag = null,
                FileIdTileImage = null,
                HagueStatusId = await _nodeIdReader.ReadAsync(Constants.PPL, 41213),
                ResidencyRequirements = null,
                AgeRequirements = null,
                HealthRequirements = null,
                IncomeRequirements = null,
                MarriageRequirements = null,
                OtherRequirements = null,
            };
        }
        await reader.CloseAsync();
    }
}
