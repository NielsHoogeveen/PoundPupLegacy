using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static IEnumerable<TypeOfAbuse> GetTypesOfAbuse()
        {
            return new List<TypeOfAbuse>
            {
                new TypeOfAbuse
                {
                    Id = NON_LETHAL_PHYSICAL_ABUSE,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Non-lethal physical abuse",
                    NodeStatusId = 1,
                    NodeTypeId = 39,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Non-lethal physical abuse",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "non-lethal physical abuse",
                            ParentNames = new List<string>{ "physical abuse"},
                        },
                    },
                },
                new TypeOfAbuse
                {
                    Id = LETHAL_PHYSICAL_ABUSE,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Lethal physical abuse",
                    NodeStatusId = 1,
                    NodeTypeId = 39,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Lethal physical abuse",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "lethal physical abuse",
                            ParentNames = new List<string>{ "physical abuse", "lethal abuse"},
                        },
                    },
                },
                new TypeOfAbuse
                {
                    Id = PHYSICAL_EXPLOITATION,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Physical exploitation",
                    NodeStatusId = 1,
                    NodeTypeId = 39,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Physical exploitation",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "physical exploitation",
                            ParentNames = new List<string>{ "explotation"},
                        },
                    },
                },
                new TypeOfAbuse
                {
                    Id = SEXUAL_ABUSE,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Sexual abuse",
                    NodeStatusId = 1,
                    NodeTypeId = 39,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Sexual abuse",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "sexual abuse",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new TypeOfAbuse
                {
                    Id = SEXUAL_EXPLOITATION,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Sexual exploitation",
                    NodeStatusId = 1,
                    NodeTypeId = 39,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Sexual exploitation",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "sexual exploitation",
                            ParentNames = new List<string>{ "explotation"},
                        },
                    },
                },
                new TypeOfAbuse
                {
                    Id = NON_LETHAL_NEGLECT,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Non-lethal neglect",
                    NodeStatusId = 1,
                    NodeTypeId = 39,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Non-lethal neglect",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "non-lethal neglect",
                            ParentNames = new List<string>{ "neglect"},
                        },
                    },
                },
                new TypeOfAbuse
                {
                    Id = LETHAL_NEGLECT,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Lethal neglect",
                    NodeStatusId = 1,
                    NodeTypeId = 39,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Lethal neglect",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "lethal neglect",
                            ParentNames = new List<string>{ "lethal abuse", "neglect"},
                        },
                    },
                },
                new TypeOfAbuse
                {
                    Id = NON_LETHAL_DEPRIVATION,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Non-lethal deprivation",
                    NodeStatusId = 1,
                    NodeTypeId = 39,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Non-lethal deprivation",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "Non-lethal deprivation",
                            ParentNames = new List<string>{ "deprivation"},
                        },
                    },
                },
                new TypeOfAbuse
                {
                    Id = LETHAL_DEPRIVATION,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Lethal deprivation",
                    NodeStatusId = 1,
                    NodeTypeId = 39,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Lethal deprivation",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "lethal deprivation",
                            ParentNames = new List<string>{ "lethal abuse", "deprivation"},
                        },
                    },
                },
                new TypeOfAbuse
                {
                    Id = ECONOMIC_EXPLOITATION,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Economic exploitation",
                    NodeStatusId = 1,
                    NodeTypeId = 39,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Economic exploitation",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "economic exploitation",
                            ParentNames = new List<string>{ "exploitation"},
                        },
                    },
                },
                new TypeOfAbuse
                {
                    Id = VERBAL_ABUSE,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Verbal abuse",
                    NodeStatusId = 1,
                    NodeTypeId = 39,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Economic exploitation",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "economic exploitation",
                            ParentNames = new List<string>{ "exploitation"},
                        },
                    },

                },
                new TypeOfAbuse
                {
                    Id = MEDICAL_ABUSE,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Medical abuse",
                    NodeStatusId = 1,
                    NodeTypeId = 39,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Medical abuse",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "medical abuse",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new TypeOfAbuse
                {
                    Id = DEATH_BY_UNKNOWN_CAUSE,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Death by unknown cause",
                    NodeStatusId = 1,
                    NodeTypeId = 39,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = CHILD_PLACEMENT_TYPE,
                            Name = "Death by unknown cause",
                            ParentNames = new List<string>(),
                        },
                    },
                },
            };
        }
        private static void MigrateTypesOfAbuse(NpgsqlConnection connection)
        {
            TypeOfAbuseCreator.Create(GetTypesOfAbuse(), connection);
        }

    }
}
