using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal sealed class SubdivisionTypeMigrator : PPLMigrator
{

    public SubdivisionTypeMigrator(MySqlToPostgresConverter converter) : base(converter) { }

    protected override string Name => "subdivision types";

    private static async IAsyncEnumerable<SubdivisionType> GetSubdivisionTypes()
    {
        await Task.CompletedTask;

        var names = new List<string> {
            "Administered area",
            "Administration",
            "Administrative region",
            "Atol",
            "Autonomous city",
            "Autonomous community",
            "Autonomous district",
            "Autonomous municipality",
            "Autonomous province",
            "Autonomous region",
            "Autonomous republic",
            "Autonomous territorial unit",
            "Borough",
            "Canton",
            "Capital",
            "Capital city",
            "Capital district",
            "Capital region",
            "Capital territory",
            "City",
            "City corporation",
            "City with county rights",
            "Commune",
            "Council area",
            "Country",
            "County",
            "Department",
            "Dependency",
            "District",
            "Division",
            "Emirate",
            "European collectivity",
            "Federal dependency",
            "Federal district",
            "Federal entity",
            "Federal territory",
            "Free municipal consortium",
            "Geographical region",
            "Geographical unit",
            "Governorate",
            "Island",
            "Island authority",
            "Island chain",
            "Island council",
            "Local council",
            "Metropolitan administration",
            "Metropolitan collectivity with special status",
            "Metropolitan departement",
            "Metropolitan city",
            "Metropolitan region",
            "Municipality",
            "Outlying area",
            "Overseas collectivity",
            "Parish",
            "Popularate",
            "Prefecture",
            "Province",
            "Quarter",
            "Rayon",
            "Region",
            "Republic",
            "Rural municipality",
            "Sector",
            "Special administrative city",
            "Special administrative region",
            "Special municipality",
            "Special region",
            "Special self-governing province",
            "Special self-governing city",
            "State",
            "State city",
            "Territorial unit",
            "Territory",
            "Town",
            "Town council",
            "Union territory",
            "Voivodeship",
            "Ward",
            "Unitary authority",
            "Urban municipality",
        };

        foreach (var name in names) {
            yield return new SubdivisionType {
                Id = null,
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = name,
                OwnerId = Constants.OWNER_GEOGRAPHY,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = null
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = null
                    }
                },
                NodeTypeId = 51,
                Description = "",
                FileIdTileImage = null,
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.OWNER_GEOGRAPHY,
                        Name = Constants.VOCABULARY_SUBDIVISION_TYPE,
                        TermName = name,
                        ParentNames = new List<string>(),
                    },
                },
            };

        }
    }
    protected override async Task MigrateImpl()
    {
        await SubdivisionTypeCreator.CreateAsync(GetSubdivisionTypes(), _postgresConnection);
    }
}
