﻿using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal sealed class FamilySizeMigrator : Migrator
{

    public FamilySizeMigrator(MySqlToPostgresConverter converter) : base(converter) { }

    protected override string Name => "family sizes";

    private static async IAsyncEnumerable<FamilySize> GetFamilySizes()
    {
        await Task.CompletedTask;
        yield return new FamilySize
        {
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
                        TenantId = 1,
                        PublicationStatusId = 1,
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
        yield return new FamilySize
        {
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
                        TenantId = 1,
                        PublicationStatusId = 1,
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
        yield return new FamilySize
        {
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
                        TenantId = 1,
                        PublicationStatusId = 1,
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
        yield return new FamilySize
        {
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
                        TenantId = 1,
                        PublicationStatusId = 1,
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
        await FamilySizeCreator.CreateAsync(GetFamilySizes(), _postgresConnection);
    }
}