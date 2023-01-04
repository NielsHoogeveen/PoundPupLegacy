using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static IEnumerable<FamilySize> GetFamilySizes()
        {
            return new List<FamilySize>
            {
                new FamilySize
                {
                    Id = ONE_TO_FOUR,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "1 to 4",
                    NodeStatusId = 1,
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
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "4 to 8",
                    NodeStatusId = 1,
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
                    Id = EIGHT_TO_TWELVE,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "8 to 12",
                    NodeStatusId = 1,
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
                    Id = MORE_THAN_TWELVE,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "more than 12",
                    NodeStatusId = 1,
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
            await FamilySizeCreator.CreateAsync(GetFamilySizes().ToAsyncEnumerable(), connection);
        }


    }
}
