namespace PoundPupLegacy.Convert;

internal sealed class FamilySizeMigrator(
        IDatabaseConnections databaseConnections,
        IEntityCreatorFactory<EventuallyIdentifiableFamilySize> familySizeCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "family sizes";

    private static async IAsyncEnumerable<NewFamilySize> GetFamilySizes()
    {
        await Task.CompletedTask;
        yield return new NewFamilySize {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "1 to 4",
            OwnerId = Constants.OWNER_CASES,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
            {
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.ONE_TO_FOUR
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.ONE_TO_FOUR
                }
            },
            NodeTypeId = 28,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_CASES,
                    Name = Constants.VOCABULARY_FAMILY_SIZE,
                    TermName = "1 to 4",
                    ParentNames = new List<string>(),
                },
            },
            NodeTermIds = new List<int>(),
        };
        yield return new NewFamilySize {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "4 to 8",
            OwnerId = Constants.OWNER_CASES,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
            {
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.FOUR_TO_EIGHT
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.FOUR_TO_EIGHT
                }
            },
            NodeTypeId = 28,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_CASES,
                    Name = Constants.VOCABULARY_FAMILY_SIZE,
                    TermName = "4 to 8",
                    ParentNames = new List<string>(),
                },
            },
            NodeTermIds = new List<int>(),
        };
        yield return new NewFamilySize {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "8 to 12",
            OwnerId = Constants.OWNER_CASES,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
            {
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.EIGHT_TO_TWELVE
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.EIGHT_TO_TWELVE
                }
            },
            NodeTypeId = 28,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.OWNER_CASES,
                        Name = Constants.VOCABULARY_FAMILY_SIZE,
                        TermName = "8 to 12",
                        ParentNames = new List<string>(),
                    },
                },
            NodeTermIds = new List<int>(),
        };
        yield return new NewFamilySize {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "more than 12",
            OwnerId = Constants.OWNER_CASES,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
            {
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.MORE_THAN_TWELVE
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.MORE_THAN_TWELVE
                }
            },
            NodeTypeId = 28,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_CASES,
                    Name = Constants.VOCABULARY_FAMILY_SIZE,
                    TermName = "more than 12",
                    ParentNames = new List<string>(),
                },
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = "mega families",
                    ParentNames = new List<string>(),
                },
            },
            NodeTermIds = new List<int>(),
        };
    }
    protected override async Task MigrateImpl()
    {
        await using var familySizeCreator = await familySizeCreatorFactory.CreateAsync(_postgresConnection);
        await familySizeCreator.CreateAsync(GetFamilySizes());
    }
}
