using PoundPupLegacy.DomainModel;
using PoundPupLegacy.DomainModel.Creators;
using PoundPupLegacy.DomainModel.Readers;

namespace PoundPupLegacy.Convert;

internal sealed class UnitedStatesPoliticalPartyAffliationMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderByUrlIdFactory,
        IEntityCreatorFactory<UnitedStatesPoliticalPartyAffiliation.ToCreate> unitedStatesPoliticalPartyAffliationCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "united states political party affilition";

    private async IAsyncEnumerable<UnitedStatesPoliticalPartyAffiliation.ToCreate> GetUnitedStatesPoliticalPartyAffliations(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader
    )
    {
        var vocabularyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_UNITED_STATES_POLITICAL_PARTY_AFFILITION_TYPE

        });
        yield return new UnitedStatesPoliticalPartyAffiliation.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.ForCreate {
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = Constants.DEMOCRAT_NAME,
                OwnerId = Constants.OWNER_PARTIES,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode.ToCreate.ForNewNode>
                {
                    new TenantNode.ToCreate.ForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.DEMOCRAT
                    },
                    new TenantNode.ToCreate.ForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.DEMOCRAT
                    }
                },
                NodeTypeId = 62,
                TermIds = new List<int>(),
            },
            NameableDetails = new NameableDetails.ForCreate {
                Description = "",
                FileIdTileImage = null,
                Terms = new List<Term.ToCreateForNewNameable>
                {
                    new Term.ToCreateForNewNameable
                    {
                        Identification = new Identification.Possible {
                            Id= null
                        },
                        VocabularyId = vocabularyId,
                        Name = Constants.DEMOCRAT_NAME,
                        ParentTermIds = new List<int>(),
                    },
                },
            },
            UnitedStatesPoliticalPartyAffliationDetails = new UnitedStatesPoliticalPartyAffliationDetails {
                UnitedStatesPoliticalPartyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = Constants.DEMOCRATIC_PARTY
                }),
            },
        };

        yield return new UnitedStatesPoliticalPartyAffiliation.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.ForCreate {
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = Constants.REPUBLICAN_NAME,
                OwnerId = Constants.OWNER_PARTIES,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode.ToCreate.ForNewNode>
                {
                    new TenantNode.ToCreate.ForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.REPUBLICAN
                    },
                    new TenantNode.ToCreate.ForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.REPUBLICAN
                    }
                },
                NodeTypeId = 62,
                TermIds = new List<int>(),
            },
            NameableDetails = new NameableDetails.ForCreate {
                Description = "",
                FileIdTileImage = null,
                Terms = new List<Term.ToCreateForNewNameable>
                {
                    new Term.ToCreateForNewNameable
                    {
                        Identification = new Identification.Possible {
                            Id= null
                        },
                        VocabularyId = vocabularyId,
                        Name = Constants.REPUBLICAN_NAME,
                        ParentTermIds = new List<int>(),
                    },
                },
            },
            UnitedStatesPoliticalPartyAffliationDetails = new UnitedStatesPoliticalPartyAffliationDetails {
                UnitedStatesPoliticalPartyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = Constants.REPUBLICAN_PARTY
                }),
            }
        };
        yield return new UnitedStatesPoliticalPartyAffiliation.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.ForCreate {
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = Constants.INDEPENDENT_NAME,
                OwnerId = Constants.OWNER_PARTIES,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode.ToCreate.ForNewNode>
                {
                    new TenantNode.ToCreate.ForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.INDEPENDENT
                    },
                    new TenantNode.ToCreate.ForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.INDEPENDENT
                    }
                },
                NodeTypeId = 62,
                TermIds = new List<int>(),
            },
            NameableDetails = new NameableDetails.ForCreate {
                Description = "",
                FileIdTileImage = null,
                Terms = new List<Term.ToCreateForNewNameable>
                {
                    new Term.ToCreateForNewNameable
                    {
                        Identification = new Identification.Possible {
                            Id= null
                        },
                        VocabularyId = vocabularyId,
                        Name = Constants.INDEPENDENT_NAME,
                        ParentTermIds = new List<int>(),
                    },
                },
            },
            UnitedStatesPoliticalPartyAffliationDetails = new UnitedStatesPoliticalPartyAffliationDetails {
                UnitedStatesPoliticalPartyId = null,
            },
        };
        yield return new UnitedStatesPoliticalPartyAffiliation.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.ForCreate {
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = Constants.POPULAR_DEMOCRAT_NAME,
                OwnerId = Constants.OWNER_PARTIES,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode.ToCreate.ForNewNode>
                {
                    new TenantNode.ToCreate.ForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.POPULAR_DEMOCRAT
                    },
                    new TenantNode.ToCreate.ForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.POPULAR_DEMOCRAT
                    }
                },
                NodeTypeId = 62,
                TermIds = new List<int>(),
            },
            NameableDetails = new NameableDetails.ForCreate {
                Description = "",
                FileIdTileImage = null,
                Terms = new List<Term.ToCreateForNewNameable>
                {
                    new Term.ToCreateForNewNameable
                    {
                        Identification = new Identification.Possible {
                            Id= null
                        },
                        VocabularyId = vocabularyId,
                        Name = Constants.POPULAR_DEMOCRAT_NAME,
                        ParentTermIds = new List<int>(),
                    },
                },
            },
            UnitedStatesPoliticalPartyAffliationDetails = new UnitedStatesPoliticalPartyAffliationDetails {
                UnitedStatesPoliticalPartyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = Constants.POPULAR_DEMOCRAT_PARTY
                }),
            }
        };
        yield return new UnitedStatesPoliticalPartyAffiliation.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.ForCreate {
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = Constants.LIBERTARIAN_NAME,
                OwnerId = Constants.OWNER_PARTIES,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode.ToCreate.ForNewNode>
                {
                    new TenantNode.ToCreate.ForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.LIBERTARIAN
                    },
                    new TenantNode.ToCreate.ForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.LIBERTARIAN
                    }
                },
                NodeTypeId = 62,
                TermIds = new List<int>(),
            },
            NameableDetails = new NameableDetails.ForCreate {
                Description = "",
                FileIdTileImage = null,
                Terms = new List<Term.ToCreateForNewNameable>
                {
                    new Term.ToCreateForNewNameable
                    {
                        Identification = new Identification.Possible {
                            Id = null,
                        },
                        VocabularyId = vocabularyId,
                        Name = Constants.LIBERTARIAN_NAME,
                        ParentTermIds = new List<int>(),
                    },
                },
            },
            UnitedStatesPoliticalPartyAffliationDetails = new UnitedStatesPoliticalPartyAffliationDetails {
                UnitedStatesPoliticalPartyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = Constants.LIBERTARIAN_PARTY
                }),
            }
        };
    }
    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderByUrlIdFactory.CreateAsync(_postgresConnection);
        await using var unitedStatesPoliticalPartyAffliationCreator = await unitedStatesPoliticalPartyAffliationCreatorFactory.CreateAsync(_postgresConnection);
        await unitedStatesPoliticalPartyAffliationCreator.CreateAsync(GetUnitedStatesPoliticalPartyAffliations(nodeIdReader));
    }
}
