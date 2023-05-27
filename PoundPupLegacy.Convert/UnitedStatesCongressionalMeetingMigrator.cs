namespace PoundPupLegacy.Convert;

internal sealed class UnitedStatesCongressionalMeetingMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
        IEntityCreatorFactory<EventuallyIdentifiableUnitedStatesCongressionalMeeting> unitedStatesCongressionalMeetingCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "united states congressional meetings";

    private async IAsyncEnumerable<NewUnitedStatesCongressionalMeeting> ReadUnitedStatesCongressionalMeetingCsv(
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
            yield return new NewUnitedStatesCongressionalMeeting {
                Id = null,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Terms = new List<NewTermForNewNameable>
                {
                    new NewTermForNewNameable
                    {
                        VocabularyId = vocabularyId,
                        Name = title,
                        ParentTermIds = parentTermIds,
                    }
                },
                Description = "",
                FileIdTileImage = null,
                NodeTypeId = 52,
                OwnerId = Constants.OWNER_GEOGRAPHY,
                AuthoringStatusId = 1,
                TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = null
                    }
                },
                PublisherId = 2,
                Title = title,
                DateRange = new DateTimeRange(startDate, endDate),
                Number = number,
                TermIds = new List<int>(),
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
