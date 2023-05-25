namespace PoundPupLegacy.Convert;

internal sealed class UnitedStatesCongressionalMeetingMigrator(
        IDatabaseConnections databaseConnections,
        IEntityCreatorFactory<EventuallyIdentifiableUnitedStatesCongressionalMeeting> unitedStatesCongressionalMeetingCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "united states congressional meetings";

    private async IAsyncEnumerable<NewUnitedStatesCongressionalMeeting> ReadUnitedStatesCongressionalMeetingCsv()
    {

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
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.OWNER_SYSTEM,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = title,
                        ParentNames = new List<string>{ "United States Congress" },
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
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = null
                    }
                },
                PublisherId = 2,
                Title = title,
                DateRange = new DateTimeRange(startDate, endDate),
                Number = number,
                NodeTermIds = new List<int>(),
            };
        }
    }

    protected override async Task MigrateImpl()
    {
        await using var unitedStatesCongressionalMeetingCreator = await unitedStatesCongressionalMeetingCreatorFactory.CreateAsync(_postgresConnection);
        await unitedStatesCongressionalMeetingCreator.CreateAsync(ReadUnitedStatesCongressionalMeetingCsv());
    }
}
