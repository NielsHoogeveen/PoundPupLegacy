namespace PoundPupLegacy.Convert;

internal sealed class CasePartyTypeMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IEntityCreatorFactory<EventuallyIdentifiableCasePartyType> casePartyTypeCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "case relation types";
    protected override async Task MigrateImpl()
    {
        await using var casePartyTypeCreator = await casePartyTypeCreatorFactory.CreateAsync(_postgresConnection);
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await casePartyTypeCreator.CreateAsync(GetCaseRelationTypes(nodeIdReader));
    }

    internal static async IAsyncEnumerable<NewCasePartyType> GetCaseRelationTypes(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var vocabularyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_CASE_PARTY_TYPE
        });

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
                    VocabularyId = vocabularyId,
                    TermName = Constants.HOMESTUDY_CASE_TYPE_NAME,
                    ParentTermIds = new List<int>(),
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
                    VocabularyId = vocabularyId,
                    TermName = Constants.PLACEMENT_CASE_TYPE_NAME,
                    ParentTermIds = new List<int>(),
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
                    VocabularyId = vocabularyId,
                    TermName = Constants.POSTPLACEMENT_CASE_TYPE_NAME,
                    ParentTermIds = new List<int>(),
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
                    VocabularyId = vocabularyId,
                    TermName = Constants.FACILITATION_CASE_TYPE_NAME,
                    ParentTermIds = new List<int>(),
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
                    VocabularyId = vocabularyId,
                    TermName = Constants.INSTITUTION_CASE_TYPE_NAME,
                    ParentTermIds = new List<int>(),
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
                    VocabularyId = vocabularyId,
                    TermName = Constants.THERAPY_CASE_TYPE_NAME,
                    ParentTermIds = new List<int>(),
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
                    VocabularyId = vocabularyId,
                    TermName = Constants.AUTHORITIES_CASE_TYPE_NAME,
                    ParentTermIds = new List<int>(),
                }
            },
            NodeTermIds = new List<int>(),
        };
    }
}
