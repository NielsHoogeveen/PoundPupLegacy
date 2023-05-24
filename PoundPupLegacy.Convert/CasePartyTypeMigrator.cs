namespace PoundPupLegacy.Convert;

internal sealed class CasePartyTypeMigrator(
        IDatabaseConnections databaseConnections,
        INameableCreatorFactory<EventuallyIdentifiableCasePartyType> casePartyTypeCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "case relation types";
    protected override async Task MigrateImpl()
    {
        await using var casePartyTypeCreator = await casePartyTypeCreatorFactory.CreateAsync(_postgresConnection);
        await casePartyTypeCreator.CreateAsync(GetCaseRelationTypes());
    }

    internal static async IAsyncEnumerable<NewCasePartyType> GetCaseRelationTypes()
    {
        await Task.CompletedTask;
        var now = DateTime.Now;
        yield return new NewCasePartyType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.HOMESTUDY_CASE_TYPE_NAME,
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
                    UrlId = Constants.HOMESTUDY_CASE_TYPE
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.HOMESTUDY_CASE_TYPE
                }
            },
            NodeTypeId = 1,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_CASES,
                    Name = Constants.VOCABULARY_CASE_PARTY_TYPE,
                    TermName = Constants.HOMESTUDY_CASE_TYPE_NAME,
                    ParentNames = new List<string>(),
                }
            },
            NodeTermIds = new List<int>(),
        };
        yield return new NewCasePartyType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.PLACEMENT_CASE_TYPE_NAME,
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
                    UrlId = Constants.PLACEMENT_CASE_TYPE
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.PLACEMENT_CASE_TYPE
                }
            },
            NodeTypeId = 1,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_CASES,
                    Name = Constants.VOCABULARY_CASE_PARTY_TYPE,
                    TermName = Constants.PLACEMENT_CASE_TYPE_NAME,
                    ParentNames = new List<string>(),
                }
            },
            NodeTermIds = new List<int>(),
        };
        yield return new NewCasePartyType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.POSTPLACEMENT_CASE_TYPE_NAME,
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
                    UrlId = Constants.POSTPLACEMENT_CASE_TYPE
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.POSTPLACEMENT_CASE_TYPE
                }
            },
            NodeTypeId = 1,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_CASES,
                    Name = Constants.VOCABULARY_CASE_PARTY_TYPE,
                    TermName = Constants.POSTPLACEMENT_CASE_TYPE_NAME,
                    ParentNames = new List<string>(),
                }
            },
            NodeTermIds = new List<int>(),
        };
        yield return new NewCasePartyType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.FACILITATION_CASE_TYPE_NAME,
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
                    UrlId = Constants.FACILITATION_CASE_TYPE
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.FACILITATION_CASE_TYPE
                }
            },
            NodeTypeId = 1,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_CASES,
                    Name = Constants.VOCABULARY_CASE_PARTY_TYPE,
                    TermName = Constants.FACILITATION_CASE_TYPE_NAME,
                    ParentNames = new List<string>(),
                }
            },
            NodeTermIds = new List<int>(),
        };
        yield return new NewCasePartyType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.INSTITUTION_CASE_TYPE_NAME,
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
                    UrlId = Constants.INSTITUTION_CASE_TYPE
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.INSTITUTION_CASE_TYPE
                }
            },
            NodeTypeId = 1,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_CASES,
                    Name = Constants.VOCABULARY_CASE_PARTY_TYPE,
                    TermName = Constants.INSTITUTION_CASE_TYPE_NAME,
                    ParentNames = new List<string>(),
                }
            },
            NodeTermIds = new List<int>(),
        };
        yield return new NewCasePartyType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.THERAPY_CASE_TYPE_NAME,
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
                    UrlId = Constants.THERAPY_CASE_TYPE
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.THERAPY_CASE_TYPE
                }
            },
            NodeTypeId = 1,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_CASES,
                    Name = Constants.VOCABULARY_CASE_PARTY_TYPE,
                    TermName = Constants.THERAPY_CASE_TYPE_NAME,
                    ParentNames = new List<string>(),
                }
            },
            NodeTermIds = new List<int>(),
        };
        yield return new NewCasePartyType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.AUTHORITIES_CASE_TYPE_NAME,
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
                    UrlId = Constants.AUTHORITIES_CASE_TYPE
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.AUTHORITIES_CASE_TYPE
                }
            },
            NodeTypeId = 1,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_CASES,
                    Name = Constants.VOCABULARY_CASE_PARTY_TYPE,
                    TermName = Constants.AUTHORITIES_CASE_TYPE_NAME,
                    ParentNames = new List<string>(),
                }
            },
            NodeTermIds = new List<int>(),
        };
    }
}
