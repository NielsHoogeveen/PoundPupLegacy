using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal sealed class UnitedStatesPoliticalPartyAffliationMigrator : PPLMigrator
{

    public UnitedStatesPoliticalPartyAffliationMigrator(MySqlToPostgresConverter converter) : base(converter) { }

    protected override string Name => "united states political party affilition";

    private async IAsyncEnumerable<UnitedStatesPoliticalPartyAffliation> GetUnitedStatesPoliticalPartyAffliations()
    {

        yield return new UnitedStatesPoliticalPartyAffliation {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.DEMOCRAT_NAME,
            OwnerId = Constants.OWNER_PARTIES,
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
                        UrlId = Constants.DEMOCRAT
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.DEMOCRAT
                    }
                },
            NodeTypeId = 62,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.OWNER_PARTIES,
                        Name = Constants.VOCABULARY_UNITED_STATES_POLITICAL_PARTY_AFFILITION_TYPE,
                        TermName = Constants.DEMOCRAT_NAME,
                        ParentNames = new List<string>(),
                    },
                },
            UnitedStatesPoliticalPartyId = await _nodeIdReader.ReadAsync(Constants.PPL, Constants.DEMOCRATIC_PARTY)

        };

        yield return new UnitedStatesPoliticalPartyAffliation {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.REPUBLICAN_NAME,
            OwnerId = Constants.OWNER_PARTIES,
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
                        UrlId = Constants.REPUBLICAN
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.REPUBLICAN
                    }
                },
            NodeTypeId = 62,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.OWNER_PARTIES,
                        Name = Constants.VOCABULARY_UNITED_STATES_POLITICAL_PARTY_AFFILITION_TYPE,
                        TermName = Constants.REPUBLICAN_NAME,
                        ParentNames = new List<string>(),
                    },
                },
            UnitedStatesPoliticalPartyId = await _nodeIdReader.ReadAsync(Constants.PPL, Constants.REPUBLICAN_PARTY)
        };
        yield return new UnitedStatesPoliticalPartyAffliation {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.INDEPENDENT_NAME,
            OwnerId = Constants.OWNER_PARTIES,
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
                        UrlId = Constants.INDEPENDENT
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.INDEPENDENT
                    }
                },
            NodeTypeId = 62,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.OWNER_PARTIES,
                        Name = Constants.VOCABULARY_UNITED_STATES_POLITICAL_PARTY_AFFILITION_TYPE,
                        TermName = Constants.INDEPENDENT_NAME,
                        ParentNames = new List<string>(),
                    },
                },
            UnitedStatesPoliticalPartyId = null
        };
        yield return new UnitedStatesPoliticalPartyAffliation {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.POPULAR_DEMOCRAT_NAME,
            OwnerId = Constants.OWNER_PARTIES,
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
                        UrlId = Constants.POPULAR_DEMOCRAT
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.POPULAR_DEMOCRAT
                    }
                },
            NodeTypeId = 62,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.OWNER_PARTIES,
                        Name = Constants.VOCABULARY_UNITED_STATES_POLITICAL_PARTY_AFFILITION_TYPE,
                        TermName = Constants.POPULAR_DEMOCRAT_NAME,
                        ParentNames = new List<string>(),
                    },
                },
            UnitedStatesPoliticalPartyId = await _nodeIdReader.ReadAsync(Constants.PPL, Constants.POPULAR_DEMOCRAT_PARTY)
        };
        yield return new UnitedStatesPoliticalPartyAffliation {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.LIBERTARIAN_NAME,
            OwnerId = Constants.OWNER_PARTIES,
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
                        UrlId = Constants.LIBERTARIAN
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.LIBERTARIAN
                    }
                },
            NodeTypeId = 62,
            Description = "",
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.OWNER_PARTIES,
                        Name = Constants.VOCABULARY_UNITED_STATES_POLITICAL_PARTY_AFFILITION_TYPE,
                        TermName = Constants.LIBERTARIAN_NAME,
                        ParentNames = new List<string>(),
                    },
                },
            UnitedStatesPoliticalPartyId = await _nodeIdReader.ReadAsync(Constants.PPL, Constants.LIBERTARIAN_PARTY)
        };
    }
    protected override async Task MigrateImpl()
    {

        await UnitedStatesPoliticalPartyAffliationCreator.CreateAsync(GetUnitedStatesPoliticalPartyAffliations(), _postgresConnection);
    }
}
