namespace PoundPupLegacy.Convert;

internal sealed class UnitedStatesPoliticalPartyAffliationMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderByUrlIdFactory,
        IEntityCreatorFactory<EventuallyIdentifiableUnitedStatesPoliticalPartyAffliation> unitedStatesPoliticalPartyAffliationCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "united states political party affilition";

    private async IAsyncEnumerable<UnitedStatesPoliticalPartyAffiliationToCreate> GetUnitedStatesPoliticalPartyAffliations(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader
    )
    {
        var vocabularyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_UNITED_STATES_POLITICAL_PARTY_AFFILITION_TYPE

        });
        yield return new UnitedStatesPoliticalPartyAffiliationToCreate {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.DEMOCRAT_NAME,
            OwnerId = Constants.OWNER_PARTIES,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.DEMOCRAT
                    },
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.DEMOCRAT
                    }
                },
            NodeTypeId = 62,
            Description = "",
            FileIdTileImage = null,
            Terms = new List<NewTermForNewNameable>
                {
                    new NewTermForNewNameable
                    {
                        VocabularyId = vocabularyId,
                        Name = Constants.DEMOCRAT_NAME,
                        ParentTermIds = new List<int>(),
                    },
                },
            UnitedStatesPoliticalPartyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = Constants.DEMOCRATIC_PARTY
            }),
            TermIds = new List<int>(),
        };

        yield return new UnitedStatesPoliticalPartyAffiliationToCreate {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.REPUBLICAN_NAME,
            OwnerId = Constants.OWNER_PARTIES,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.REPUBLICAN
                    },
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.REPUBLICAN
                    }
                },
            NodeTypeId = 62,
            Description = "",
            FileIdTileImage = null,
            Terms = new List<NewTermForNewNameable>
                {
                    new NewTermForNewNameable
                    {
                        VocabularyId = vocabularyId,
                        Name = Constants.REPUBLICAN_NAME,
                        ParentTermIds = new List<int>(),
                    },
                },
            UnitedStatesPoliticalPartyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = Constants.REPUBLICAN_PARTY
            }),
            TermIds = new List<int>(),
        };
        yield return new UnitedStatesPoliticalPartyAffiliationToCreate {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.INDEPENDENT_NAME,
            OwnerId = Constants.OWNER_PARTIES,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.INDEPENDENT
                    },
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.INDEPENDENT
                    }
                },
            NodeTypeId = 62,
            Description = "",
            FileIdTileImage = null,
            Terms = new List<NewTermForNewNameable>
                {
                    new NewTermForNewNameable
                    {
                        VocabularyId = vocabularyId,
                        Name = Constants.INDEPENDENT_NAME,
                        ParentTermIds = new List<int>(),
                    },
                },
            UnitedStatesPoliticalPartyId = null,
            TermIds = new List<int>(),
        };
        yield return new UnitedStatesPoliticalPartyAffiliationToCreate {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.POPULAR_DEMOCRAT_NAME,
            OwnerId = Constants.OWNER_PARTIES,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.POPULAR_DEMOCRAT
                    },
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.POPULAR_DEMOCRAT
                    }
                },
            NodeTypeId = 62,
            Description = "",
            FileIdTileImage = null,
            Terms = new List<NewTermForNewNameable>
                {
                    new NewTermForNewNameable
                    {
                        VocabularyId = vocabularyId,
                        Name = Constants.POPULAR_DEMOCRAT_NAME,
                        ParentTermIds = new List<int>(),
                    },
                },
            UnitedStatesPoliticalPartyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = Constants.POPULAR_DEMOCRAT_PARTY
            }),
            TermIds = new List<int>(),
        };
        yield return new UnitedStatesPoliticalPartyAffiliationToCreate {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.LIBERTARIAN_NAME,
            OwnerId = Constants.OWNER_PARTIES,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.LIBERTARIAN
                    },
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.LIBERTARIAN
                    }
                },
            NodeTypeId = 62,
            Description = "",
            FileIdTileImage = null,
            Terms = new List<NewTermForNewNameable>
                {
                    new NewTermForNewNameable
                    {
                        VocabularyId = vocabularyId,
                        Name = Constants.LIBERTARIAN_NAME,
                        ParentTermIds = new List<int>(),
                    },
                },
            UnitedStatesPoliticalPartyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = Constants.LIBERTARIAN_PARTY
            }),
            TermIds = new List<int>(),
        };
    }
    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderByUrlIdFactory.CreateAsync(_postgresConnection);
        await using var unitedStatesPoliticalPartyAffliationCreator = await unitedStatesPoliticalPartyAffliationCreatorFactory.CreateAsync(_postgresConnection);
        await unitedStatesPoliticalPartyAffliationCreator.CreateAsync(GetUnitedStatesPoliticalPartyAffliations(nodeIdReader));
    }
}
