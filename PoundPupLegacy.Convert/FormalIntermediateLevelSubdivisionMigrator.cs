using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.Convert;

internal sealed class FormalIntermediateLevelSubdivisionMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermNameReaderByNameableIdRequest, string> termReaderByNameableIdFactory,
        IMandatorySingleItemDatabaseReaderFactory<NameableIdReaderByTermNameRequest, int> termReaderByNameFactory,
        IEntityCreatorFactory<FormalIntermediateLevelSubdivision.ToCreate> formalIntermediateLevelSubdivisionCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "formal intermediate level subdivisions";

    private async IAsyncEnumerable<FormalIntermediateLevelSubdivision.ToCreate> ReadFormalIntermediateLevelSubdivisionCsv(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader,
        IMandatorySingleItemDatabaseReader<TermNameReaderByNameableIdRequest, string> termReaderByNameableId,
        IMandatorySingleItemDatabaseReader<NameableIdReaderByTermNameRequest, int> termReaderByName
        )
    {

        var vocabularyIdSubdivisionType = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_SUBDIVISION_TYPE,
        });
        var vocabularyIdTopics = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });

        await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\files\FormalIntermediateLevelSubdivisions.csv").Skip(1)) {

            var parts = line.Split(new char[] { ';' }).Select(x => x.TrimStart()).ToList();
            int? id = int.Parse(parts[0]) == 0 ? null : int.Parse(parts[0]);
            var title = parts[8];
            var countryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = int.Parse(parts[7])
            });
            var countryName = (await termReaderByNameableId.ReadAsync(new TermNameReaderByNameableIdRequest {
                VocabularyId = vocabularyIdTopics,
                NameableId = countryId
            }));
            yield return new FormalIntermediateLevelSubdivision.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.ForCreate {
                    Title = title,
                    CreatedDateTime = DateTime.Parse(parts[1]),
                    ChangedDateTime = DateTime.Parse(parts[2]),
                    NodeTypeId = int.Parse(parts[4]),
                    OwnerId = Constants.OWNER_GEOGRAPHY,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreateForNewNode>
                    {
                        new TenantNode.ToCreateForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.PPL,
                            PublicationStatusId = int.Parse(parts[5]),
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = id
                        },
                        new TenantNode.ToCreateForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.CPCT,
                            PublicationStatusId = 2,
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = id < 33163 ? id : null
                        }
                    },
                    PublisherId = int.Parse(parts[6]),
                    TermIds = new List<int>(),
                },
                NameableDetails = new NameableDetails.ForCreate {
                    Description = "",
                    FileIdTileImage = null,
                    Terms = new List<NewTermForNewNameable>
                    {
                        new NewTermForNewNameable
                        {
                            Identification = new Identification.Possible {
                                Id = null,
                            },
                            VocabularyId = vocabularyIdTopics,
                            Name = title,
                            ParentTermIds = new List<int> {
                                await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                                    Name = countryName ,
                                    VocabularyId = vocabularyIdTopics
                                })
                            },
                        }
                    },
                },
                PoliticalEntityDetails =new PoliticalEntityDetails {
                    FileIdFlag = null,
                },
                SubdivisionDetails = new SubdivisionDetails {
                    CountryId = countryId,
                    Name = parts[9],
                    SubdivisionTypeId = (await termReaderByName.ReadAsync(new NameableIdReaderByTermNameRequest {
                        VocabularyId = vocabularyIdSubdivisionType,
                        Name = parts[11].Trim()
                    })),
                },
                ISOCodedSubdivisionDetails =new ISOCodedSubdivisionDetails {
                    ISO3166_2_Code = parts[10],
                },
            };
        }
    }

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termReaderByName = await termReaderByNameFactory.CreateAsync(_postgresConnection);
        await using var termReaderByNameableId = await termReaderByNameableIdFactory.CreateAsync(_postgresConnection);

        await using var formalIntermediateLevelSubdivisionCreator = await formalIntermediateLevelSubdivisionCreatorFactory.CreateAsync(_postgresConnection);
        await formalIntermediateLevelSubdivisionCreator.CreateAsync(ReadFormalIntermediateLevelSubdivisionCsv(
            nodeIdReader,
            termIdReader,
            termReaderByNameableId,
            termReaderByName
        ));

    }
}
