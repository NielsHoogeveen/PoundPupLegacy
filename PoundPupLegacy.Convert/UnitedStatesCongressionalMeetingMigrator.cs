namespace PoundPupLegacy.Convert;

internal sealed class UnitedStatesCongressionalMeetingMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
        IEntityCreatorFactory<UnitedStatesCongressionalMeeting.UnitedStatesCongressionalMeetingToCreate> unitedStatesCongressionalMeetingCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "united states congressional meetings";

    private async IAsyncEnumerable<UnitedStatesCongressionalMeeting.UnitedStatesCongressionalMeetingToCreate> ReadUnitedStatesCongressionalMeetingCsv(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader
    )
    {

        var vocabularyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });

        var parentTermIds = new List<int> {
            await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                Name = "United States Congress",
                VocabularyId = vocabularyId
            }
        )};
        await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\files\united_states_congress.csv").Skip(1)) {

            var parts = line.Split(new char[] { ';' }).Select(x => x.TrimStart()).ToList();
            var title = parts[0];
            var startDate = DateTime.Parse(parts[1]).AddHours(12);
            var endDate = DateTime.Parse(parts[2]).AddHours(12).AddMicroseconds(-1);
            var number = int.Parse(parts[3]);
            yield return new UnitedStatesCongressionalMeeting.UnitedStatesCongressionalMeetingToCreate {
                IdentificationForCreate = new Identification.IdentificationForCreate {
                    Id = null
                },
                NodeDetailsForCreate = new NodeDetails.NodeDetailsForCreate {
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    NodeTypeId = 52,
                    OwnerId = Constants.OWNER_GEOGRAPHY,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.TenantNodeToCreateForNewNode>
                    {
                        new TenantNode.TenantNodeToCreateForNewNode
                        {
                            IdentificationForCreate = new Identification.IdentificationForCreate {
                                Id = null
                            },
                            TenantId = 1,
                            PublicationStatusId = 1,
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = null
                        }
                    },
                    PublisherId = 2,
                    Title = title,
                    TermIds = new List<int>(),
                },
                NameableDetailsForCreate = new NameableDetails.NameableDetailsForCreate {
                    Terms = new List<NewTermForNewNameable>
                    {
                        new NewTermForNewNameable
                        {
                            IdentificationForCreate = new Identification.IdentificationForCreate {
                                Id= null
                            },
                            VocabularyId = vocabularyId,
                            Name = title,
                            ParentTermIds = parentTermIds,
                        }
                    },
                    Description = "",
                    FileIdTileImage = null,
                },
                UnitedStatesCongressionalMeetingDetails = new UnitedStatesCongressionalMeetingDetails {
                    DateRange = new DateTimeRange(startDate, endDate),
                    Number = number,
                },
            };
        }
    }

    protected override async Task MigrateImpl()
    {
        await using var unitedStatesCongressionalMeetingCreator = await unitedStatesCongressionalMeetingCreatorFactory.CreateAsync(_postgresConnection);
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await unitedStatesCongressionalMeetingCreator.CreateAsync(ReadUnitedStatesCongressionalMeetingCsv(nodeIdReader, termIdReader));
    }
}
