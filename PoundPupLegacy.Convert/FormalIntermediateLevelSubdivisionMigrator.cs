using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.Convert;

internal sealed class FormalIntermediateLevelSubdivisionMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
        ISingleItemDatabaseReaderFactory<TermReaderByNameableIdRequest, CreateModel.Term> termReaderByNameableIdFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, CreateModel.Term> termReaderByNameFactory,
        IEntityCreatorFactory<EventuallyIdentifiableFormalIntermediateLevelSubdivision> formalIntermediateLevelSubdivisionCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "formal intermediate level subdivisions";

    private async IAsyncEnumerable<NewFormalIntermediateLevelSubdivision> ReadFormalIntermediateLevelSubdivisionCsv(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader,
        ISingleItemDatabaseReader<TermReaderByNameableIdRequest, CreateModel.Term> termReaderByNameableId,
        IMandatorySingleItemDatabaseReader<TermReaderByNameRequest, CreateModel.Term> termReaderByName
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
            var countryName = (await termReaderByNameableId.ReadAsync(new TermReaderByNameableIdRequest {
                OwnerId = Constants.OWNER_SYSTEM,
                VocabularyName = Constants.VOCABULARY_TOPICS,
                NameableId = countryId
            }))!.Name;
            yield return new NewFormalIntermediateLevelSubdivision {
                Id = null,
                CreatedDateTime = DateTime.Parse(parts[1]),
                ChangedDateTime = DateTime.Parse(parts[2]),
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        VocabularyId = vocabularyIdTopics,
                        TermName = title,
                        ParentTermIds = new List<int> {
                            await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                                Name = countryName ,
                                VocabularyId = vocabularyIdTopics
                            })
                        },
                    }
                },
                Description = "",
                FileIdTileImage = null,
                NodeTypeId = int.Parse(parts[4]),
                OwnerId = Constants.OWNER_GEOGRAPHY,
                AuthoringStatusId = 1,
                TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = int.Parse(parts[5]),
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    },
                    new NewTenantNodeForNewNode
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
                PublisherId = int.Parse(parts[6]),
                CountryId = countryId,
                Title = title,
                Name = parts[9],
                ISO3166_2_Code = parts[10],
                FileIdFlag = null,
                SubdivisionTypeId = (await termReaderByName.ReadAsync(new TermReaderByNameRequest {
                    VocabularyId = vocabularyIdSubdivisionType,
                    Name = parts[11].Trim()
                })).NameableId,
                NodeTermIds = new List<int>(),
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
