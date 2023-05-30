namespace PoundPupLegacy.Convert;

internal sealed class SubdivisionTypeMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IEntityCreatorFactory<SubdivisionType.ToCreate> subdivisionTypeCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "subdivision types";

    private static async IAsyncEnumerable<SubdivisionType.ToCreate> GetSubdivisionTypes(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader
    )
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
        var vocabularyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_SUBDIVISION_TYPE
        });

        foreach (var name in names) {
            yield return new SubdivisionType.ToCreate {
                IdentificationForCreate = new Identification.Possible {
                    Id = null
                },
                NodeDetailsForCreate = new NodeDetails.NodeDetailsForCreate {
                    PublisherId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = name,
                    OwnerId = Constants.OWNER_GEOGRAPHY,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreateForNewNode>
                    {
                        new TenantNode.ToCreateForNewNode
                        {
                            IdentificationForCreate = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.PPL,
                            PublicationStatusId = 1,
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = null
                        },
                        new TenantNode.ToCreateForNewNode
                        {
                            IdentificationForCreate = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.CPCT,
                            PublicationStatusId = 2,
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = null
                        }
                    },
                    NodeTypeId = 51,
                    TermIds = new List<int>(),
                },
                NameableDetailsForCreate = new NameableDetails.NameableDetailsForCreate {
                    Description = "",
                    FileIdTileImage = null,
                    Terms = new List<NewTermForNewNameable>
                {
                    new NewTermForNewNameable
                    {
                        IdentificationForCreate = new Identification.Possible {
                            Id = null,
                        },
                        VocabularyId = vocabularyId,
                        Name = name,
                        ParentTermIds = new List<int>(),
                    },
                },
                }
                
            };
        }
    }
    protected override async Task MigrateImpl()
    {
        await using var subdivisionTypeCreator = await subdivisionTypeCreatorFactory.CreateAsync(_postgresConnection);
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await subdivisionTypeCreator.CreateAsync(GetSubdivisionTypes(nodeIdReader));
    }
}
