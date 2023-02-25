using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal sealed class CasePartyTypeMigrator : PPLMigrator
{
    protected override string Name => "case relation types";

    public CasePartyTypeMigrator(MySqlToPostgresConverter converter) : base(converter)
    {

    }
    protected override async Task MigrateImpl()
    {
        await CasePartyTypeCreator.CreateAsync(GetCaseRelationTypes(), _postgresConnection);
    }

    internal static async IAsyncEnumerable<CasePartyType> GetCaseRelationTypes()
    {
        await Task.CompletedTask;
        var now = DateTime.Now;
        yield return new CasePartyType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.HOMESTUDY_CASE_TYPE_NAME,
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
                        UrlId = Constants.HOMESTUDY_CASE_TYPE
                    },
                    new TenantNode
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
            }
        };
        yield return new CasePartyType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.PLACEMENT_CASE_TYPE_NAME,
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
                        UrlId = Constants.PLACEMENT_CASE_TYPE
                    },
                    new TenantNode
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
            }
        };
        yield return new CasePartyType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.POSTPLACEMENT_CASE_TYPE_NAME,
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
                        UrlId = Constants.POSTPLACEMENT_CASE_TYPE
                    },
                    new TenantNode
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
            }
        };
        yield return new CasePartyType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.FACILITATION_CASE_TYPE_NAME,
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
                        UrlId = Constants.FACILITATION_CASE_TYPE
                    },
                    new TenantNode
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
            }
        };
        yield return new CasePartyType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.INSTITUTION_CASE_TYPE_NAME,
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
                        UrlId = Constants.INSTITUTION_CASE_TYPE
                    },
                    new TenantNode
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
            }
        };
        yield return new CasePartyType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.THERAPY_CASE_TYPE_NAME,
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
                        UrlId = Constants.THERAPY_CASE_TYPE
                    },
                    new TenantNode
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
            }
        };
        yield return new CasePartyType {
            Id = null,
            PublisherId = 2,
            CreatedDateTime = now,
            ChangedDateTime = now,
            Title = Constants.AUTHORITIES_CASE_TYPE_NAME,
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
                        UrlId = Constants.AUTHORITIES_CASE_TYPE
                    },
                    new TenantNode
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
            }
        };
    }
}
