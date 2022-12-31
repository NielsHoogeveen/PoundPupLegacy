using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static IEnumerable<TypeOfAbuser> GetTypesOfAbusers()
        {
            return new List<TypeOfAbuser>
            {
                new TypeOfAbuser
                {
                    Id = ADOPTIVE_FATHER,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Adoptive father",
                    NodeStatusId = 1,
                    NodeTypeId = 40,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = TYPE_OF_ABUSER,
                            Name = "Adoptive father",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "Adoptive father",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new TypeOfAbuser
                {
                    Id = FOSTER_FATHER,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Foster father",
                    NodeStatusId = 1,
                    NodeTypeId = 40,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = TYPE_OF_ABUSER,
                            Name = "Foster father",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new TypeOfAbuser
                {
                    Id = ADOPTIVE_MOTHER,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Adoptive mother",
                    NodeStatusId = 1,
                    NodeTypeId = 40,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = TYPE_OF_ABUSER,
                            Name = "Adoptive mother",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "Adoptive mother",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new TypeOfAbuser
                {
                    Id = FOSTER_MOTHER,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Foster mother",
                    NodeStatusId = 1,
                    NodeTypeId = 40,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = TYPE_OF_ABUSER,
                            Name = "Foster mother",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new TypeOfAbuser
                {
                    Id = LEGAL_GUARDIAN,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Legal guardian",
                    NodeStatusId = 1,
                    NodeTypeId = 40,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = TYPE_OF_ABUSER,
                            Name = "Legal guardian",
                            ParentNames = new List<string>(),
                        },
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = "Legal guardian",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new TypeOfAbuser
                {
                    Id = ADOPTED_SIBLING,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Adopted sibling",
                    NodeStatusId = 1,
                    NodeTypeId = 40,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = TYPE_OF_ABUSER,
                            Name = "Adopted sibling",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new TypeOfAbuser
                {
                    Id = FOSTER_SIBLING,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Foster sibling",
                    NodeStatusId = 1,
                    NodeTypeId = 40,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = TYPE_OF_ABUSER,
                            Name = "Foster sibling",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new TypeOfAbuser
                {
                    Id = NON_ADOPTED_SIBLING,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Non-adopted sibling",
                    NodeStatusId = 1,
                    NodeTypeId = 40,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = TYPE_OF_ABUSER,
                            Name = "Non-adopted sibling",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new TypeOfAbuser
                {
                    Id = NON_FOSTERED_SIBLING,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Non-fostered sibling",
                    NodeStatusId = 1,
                    NodeTypeId = 40,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = TYPE_OF_ABUSER,
                            Name = "Non-fostered sibling",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new TypeOfAbuser
                {
                    Id = OTHER_FAMILY_MEMBER,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Other family member",
                    NodeStatusId = 1,
                    NodeTypeId = 40,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = TYPE_OF_ABUSER,
                            Name = "Other family member",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new TypeOfAbuser
                {
                    Id = OTHER_NON_FAMILY_MEMBER,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Other non-family member",
                    NodeStatusId = 1,
                    NodeTypeId = 40,
                    Description = "",
                                        VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = TYPE_OF_ABUSER,
                            Name = "Other non-family member",
                            ParentNames = new List<string>(),
                        },
                    },
                },
                new TypeOfAbuser
                {
                    Id = UNDETERMINED,
                    AccessRoleId = 1,
                    CreatedDateTime = DateTime.Now,
                    ChangedDateTime = DateTime.Now,
                    Title = "Undetermined",
                    NodeStatusId = 1,
                    NodeTypeId = 40,
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = TYPE_OF_ABUSER,
                            Name = "Undetermined",
                            ParentNames = new List<string>(),
                        },
                    }
                },
            };
        }


        private static void MigrateTypesOfAbusers(NpgsqlConnection connection)
        {
            TypeOfAbuserCreator.Create(GetTypesOfAbusers(), connection);
        }

    }
}
