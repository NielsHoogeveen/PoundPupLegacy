using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{
    private static IEnumerable<UserRole> GetUserRoles()
    {
        return new List<UserRole>
        {
            new UserRole
            {
                Id = 4,
                Name = "Member"
            },
            new UserRole
            {
                Id = 6,
                Name = "Administrators"
            },
        };
    }
    private static IEnumerable<Tenant> GetTenants()
    {
        return new List<Tenant>
        {
            new Tenant
            {
                Id = 1,
                DomainName = "poundpuplegacy.org",
                Name = "Pound Pup Legacy",
                Description = "",
                VocabularyIdTagging = null
            },
           new Tenant
            {
                Id = 5,
                DomainName = "cpctresearch.info",
                Name = "CPCT Research",
                Description = "",
                VocabularyIdTagging = null
            },

        };
    }
    private static IEnumerable<ContentSharingGroup> GetContentSharingGroups()
    {
        return new List<ContentSharingGroup>
        {
            new ContentSharingGroup
            {
                Id = 2,
                Name = "Geographical Entities",
                Description = "",
            },
            new ContentSharingGroup
            {
                Id = 3,
                Name = "Parties",
                Description = "",
            },
            new ContentSharingGroup
            {
                Id = 4,
                Name = "Cases",
                Description = "",
            },

        };
    }

    private static IEnumerable<Collective> GetCollectives()
    {
        return new List<Collective>
        {
            new Collective
            {
                Id = 72,
                Name = "Kerry and Niels"
            },
        };
    }
    private static IEnumerable<CollectiveUser> GetCollectiveUsers()
    {
        return new List<CollectiveUser>
        {
            new CollectiveUser
            {
                CollectiveId = 72,
                UserId = 2
            },
            new CollectiveUser
            {
                CollectiveId = 72,
                UserId = 3
            },
        };
    }

    private static IEnumerable<UserGroupUserRoleUser> GetUserGroupUserRoleUsers()
    {
        return new List<UserGroupUserRoleUser>
        {
            new UserGroupUserRoleUser
            {
                UserGroupId = 1,
                UserRoleId = 6,
                UserId = 2
            },
            new UserGroupUserRoleUser
            {
                UserGroupId = 1,
                UserRoleId = 6,
                UserId = 3
            },
        };
    }


    private static async Task MigrateUsers(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            var users = ReadUsers(mysqlconnection).ToList();
            var memberList = users.Select(x => new UserGroupUserRoleUser { UserGroupId = 1, UserRoleId = 4, UserId = (int)x.Id! });
            await AnonimousUserCreator.CreateAsync(connection);
            await TenantCreator.CreateAsync(GetTenants().ToAsyncEnumerable(), connection);
            await ContentSharingGroupCreator.CreateAsync(GetContentSharingGroups().ToAsyncEnumerable(), connection);
            await UserRoleCreator.CreateAsync(GetUserRoles().ToAsyncEnumerable(), connection);
            await UserCreator.CreateAsync(users.ToAsyncEnumerable(), connection);
            await CollectiveCreator.CreateAsync(GetCollectives().ToAsyncEnumerable(), connection);
            await CollectiveUserCreator.CreateAsync(GetCollectiveUsers().ToAsyncEnumerable(), connection);
            await UserGroupUserRoleUserCreator.CreateAsync(GetUserGroupUserRoleUsers().ToAsyncEnumerable(), connection);
            await UserGroupUserRoleUserCreator.CreateAsync(memberList.ToAsyncEnumerable(), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }

    }
    private static IEnumerable<User> ReadUsers(MySqlConnection mysqlconnection)
    {

        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = """
                SELECT 
                    DISTINCT u.uid id, 
                    u.name ,  
                    FROM_UNIXTIME(u.created) created, 
                    CASE 
                		WHEN ctb.field_about_me_value = '' then null
                	    ELSE ctb.field_about_me_value 
                    END about_me, 
                	CASE 
                	    WHEN ctb.field_animal_within_value = '' then null
                		ELSE ctb.field_animal_within_value
                	END animal_within,
                	CASE
                        WHEN ctb.field_relation_to_child_placeme_value  = '' then null
                        ELSE ctb.field_relation_to_child_placeme_value
                	END relation_to_child_placement,
                    u.mail email,
                    u.pass password,
                    CASE 
                	    WHEN u.picture = '' then null
                		ELSE u.picture
                    END avatar
                FROM users u
                LEFT JOIN bio b ON b.uid = u.uid
                LEFT JOIN node n2 ON n2.nid = b.nid
                LEFT JOIN node_revisions nr ON nr.nid = n2.nid AND nr.vid = n2.vid
                LEFT JOIN content_type_uprofile ctb ON ctb.nid = n2.nid AND ctb.vid = n2.vid
                WHERE u.uid IN (
                    SELECT distinct
                    u.uid 
                    FROM users u
                    JOIN node n ON n.uid = u.uid
                    WHERE n.`type` NOT IN ('uprofile', 'usernode') AND u.uid <> 0
                )
                AND u.uid <> 72
                AND (b.nid is NULL OR b.nid IN (
                    SELECT 
                        MAX(b.nid)
                    FROM bio b
                    WHERE b.uid = u.uid
                ) )
                """;

        var reader = readCommand.ExecuteReader();

        while (reader.Read())
        {
            var aboutMe = reader.IsDBNull("about_me") ? null : reader.GetString("about_me") == "" ? null : reader.GetString("about_me");
            var animalWithing = reader.IsDBNull("animal_within") ? null : reader.GetString("animal_within") == "" ? null : reader.GetString("animal_within");
            var relationToChildPlacement = reader.IsDBNull("relation_to_child_placement") ? "Other" : reader.GetString("relation_to_child_placement");
            var avatar = reader.IsDBNull("avatar") ? null : reader.GetString("avatar") == "" ? null : reader.GetString("avatar");
            yield return new User
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                CreatedDateTime = reader.GetDateTime("created"),
                AboutMe = aboutMe == "(NULL)" ? null : aboutMe,
                AnimalWithin = animalWithing == "(NULL)" ? "" : animalWithing,
                RelationToChildPlacement = relationToChildPlacement == "(NULL)" ? "Other" : relationToChildPlacement,
                Email = reader.GetString("email"),
                Password = reader.GetString("password"),
                Avatar = avatar
            };
        }
        reader.Close();
    }
}
