using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal partial class Program
{

    private static IEnumerable<FamilySize> GetFamilySizes()
    {
        return new List<FamilySize>
        {
            new FamilySize
            {
                Id = null,
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = "1 to 4",
                OwnerId = OWNER_CASES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = ONE_TO_FOUR
                    }
                },
                NodeTypeId = 28,
                Description = "",
                FileIdTileImage = null,
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = OWNER_CASES,
                        Name = VOCABULARY_CHILD_PLACEMENT_TYPE,
                        TermName = "1 to 4",
                        ParentNames = new List<string>(),
                    },
                },
            },
            new FamilySize
            {
                Id = null,
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = "4 to 8",
                OwnerId = OWNER_CASES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = FOUR_TO_EIGHT
                    }
                },
                NodeTypeId = 28,
                Description = "",
                FileIdTileImage = null,
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = OWNER_CASES,
                        Name = VOCABULARY_CHILD_PLACEMENT_TYPE,
                        TermName = "4 to 8",
                        ParentNames = new List<string>(),
                    },
                },
            },
            new FamilySize
            {
                Id = null,
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = "8 to 12",
                OwnerId = OWNER_CASES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = EIGHT_TO_TWELVE
                    }
                },
                NodeTypeId = 28,
                Description = "",
                FileIdTileImage = null,
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = OWNER_CASES,
                        Name = VOCABULARY_CHILD_PLACEMENT_TYPE,
                        TermName = "8 to 12",
                        ParentNames = new List<string>(),
                    },
                },
            },
            new FamilySize
            {
                Id = null,
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = "more than 12",
                OwnerId = OWNER_CASES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = MORE_THAN_TWELVE
                    }
                },
                NodeTypeId = 28,
                Description = "",
                FileIdTileImage = null,
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = OWNER_CASES,
                        Name = VOCABULARY_CHILD_PLACEMENT_TYPE,
                        TermName = "more than 12",
                        ParentNames = new List<string>(),
                    },
                    new VocabularyName
                    {
                        OwnerId = PPL,
                        Name = VOCABULARY_TOPICS,
                        TermName = "mega families",
                        ParentNames = new List<string>(),
                    },
                },
            },
        };
    }
    private static async Task MigrateFamilySizes(NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await FamilySizeCreator.CreateAsync(GetFamilySizes().ToAsyncEnumerable(), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }
}
