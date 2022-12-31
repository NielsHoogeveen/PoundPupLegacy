using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static IEnumerable<ChildPlacementType> GetChildPlacementTypes()
        {
            return new List<ChildPlacementType>
            {
                new ChildPlacementType
                {
                    Id = ADOPTION,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Adoption",
                    NodeStatusId = 1,
                    NodeTypeId = 27,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Adoption",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "Adoption",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new ChildPlacementType
                {
                    Id = FOSTER_CARE,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Foster care",
                    NodeStatusId = 1,
                    NodeTypeId = 27,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Foster care",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "Foster care",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new ChildPlacementType
                {
                    Id = TO_BE_ADOPTED,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "To be adopted",
                    NodeStatusId = 1,
                    NodeTypeId = 27,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "To be adopted",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new ChildPlacementType
                {
                    Id = LEGAL_GUARDIANSHIP,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Legal Guardianship",
                    NodeStatusId = 1,
                    NodeTypeId = 27,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Legal Guardianship",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "Legal Guardianship",
                            ParentNames = new List<string>(),
                        },
                    },

                },
                 new ChildPlacementType
                {
                    Id = INSTITUTION,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Institution",
                    NodeStatusId = 1,
                    NodeTypeId = 27,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Institution",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "Institution",
                            ParentNames = new List<string>(),
                        },
                    },
                },
            };
        }
        private static void MigrateChildPlacementTypes(NpgsqlConnection connection)
        {
            ChildPlacementTypeCreator.Create(GetChildPlacementTypes(), connection);
        }
    }
}
