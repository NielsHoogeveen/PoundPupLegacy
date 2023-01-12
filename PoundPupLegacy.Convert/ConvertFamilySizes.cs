using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Reflection.PortableExecutable;

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
                OwnerId = null,
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
                        VocabularyId = CHILD_PLACEMENT_TYPE,
                        Name = "1 to 4",
                        ParentNames = new List<string>(),
                    },
                },
            },
            new FamilySize
            {
                Id = FOUR_TO_EIGHT,
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = "4 to 8",
                OwnerId = null,
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
                        VocabularyId = CHILD_PLACEMENT_TYPE,
                        Name = "4 to 8",
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
                OwnerId = null,
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
                        VocabularyId = CHILD_PLACEMENT_TYPE,
                        Name = "8 to 12",
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
                OwnerId = null,
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
                        VocabularyId = CHILD_PLACEMENT_TYPE,
                        Name = "more than 12",
                        ParentNames = new List<string>(),
                    },
                    new VocabularyName
                    {
                        VocabularyId = TOPICS,
                        Name = "mega families",
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
