namespace PoundPupLegacy.Convert;

internal sealed class FamilySizeMigrator : MigratorPPL
{
    private readonly IEntityCreator<FamilySize> _familySizeCreator;
    public FamilySizeMigrator(
         IDatabaseConnections databaseConnections,
         IEntityCreator<FamilySize> familySizeCreator
     ) : base(databaseConnections)
    {
        _familySizeCreator = familySizeCreator;
    }

    protected override string Name => "family sizes";

    private static async IAsyncEnumerable<FamilySize> GetFamilySizes()
    {
        await Task.CompletedTask;
        yield return new FamilySize {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "1 to 4",
            OwnerId = Constants.OWNER_CASES,
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
                        UrlId = Constants.ONE_TO_FOUR
                    },
                    new TenantNode
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
                        Name = Constants.VOCABULARY_CHILD_PLACEMENT_TYPE,
                        TermName = "1 to 4",
                        ParentNames = new List<string>(),
                    },
                },
        };
        yield return new FamilySize {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "4 to 8",
            OwnerId = Constants.OWNER_CASES,
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
                        UrlId = Constants.FOUR_TO_EIGHT
                    },
                    new TenantNode
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
                        Name = Constants.VOCABULARY_CHILD_PLACEMENT_TYPE,
                        TermName = "4 to 8",
                        ParentNames = new List<string>(),
                    },
                },
        };
        yield return new FamilySize {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "8 to 12",
            OwnerId = Constants.OWNER_CASES,
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
                        UrlId = Constants.EIGHT_TO_TWELVE
                    },
                    new TenantNode
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
                        Name = Constants.VOCABULARY_CHILD_PLACEMENT_TYPE,
                        TermName = "8 to 12",
                        ParentNames = new List<string>(),
                    },
                },
        };
        yield return new FamilySize {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "more than 12",
            OwnerId = Constants.OWNER_CASES,
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
                        UrlId = Constants.MORE_THAN_TWELVE
                    },
                    new TenantNode
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
                        Name = Constants.VOCABULARY_CHILD_PLACEMENT_TYPE,
                        TermName = "more than 12",
                        ParentNames = new List<string>(),
                    },
                    new VocabularyName
                    {
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = "mega families",
                        ParentNames = new List<string>(),
                    },
                },
        };
    }
    protected override async Task MigrateImpl()
    {
        await _familySizeCreator.CreateAsync(GetFamilySizes(), _postgresConnection);
    }
}
