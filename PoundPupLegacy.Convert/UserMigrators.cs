using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class UserMigrator : PPLMigrator
{
    protected override string Name => "users";
    public UserMigrator(MySqlToPostgresConverter converter) : base(converter)
    {

    }

    private static async IAsyncEnumerable<UserRole> GetUserRoles()
    {
        await Task.CompletedTask;
        yield return new UserRole
        {
            Id = 4,
            Name = "Member"
        };
        yield return new UserRole
        {
            Id = 6,
            Name = "Administrators"
        };
        yield return new UserRole
        {
            Id = 11,
            Name = "Editor"
        };
        yield return new UserRole
        {
            Id = 12,
            Name = "Everyone"
        };

    }
    private static async IAsyncEnumerable<Tenant> GetTenants()
    {
        await Task.CompletedTask;
        yield return new Tenant
        {
            Id = Constants.PPL,
            DomainName = "poundpuplegacy.org",
            Name = "Pound Pup Legacy",
            Description = "",
            VocabularyIdTagging = null
        };
        yield return new Tenant
        {
            Id = Constants.CPCT,
            DomainName = "cpctresearch.info",
            Name = "CPCT Research",
            Description = "",
            VocabularyIdTagging = null
        };
    }

    private static async IAsyncEnumerable<ContentSharingGroup> GetContentSharingGroups()
    {
        await Task.CompletedTask;
        yield return new ContentSharingGroup
        {
            Id = Constants.OWNER_GEOGRAPHY,
            Name = "Geographical Entities",
            Description = "",
        };
        yield return new ContentSharingGroup
        {
            Id = Constants.OWNER_PARTIES,
            Name = "Parties",
            Description = "",
        };
        yield return new ContentSharingGroup
        {
            Id = Constants.OWNER_CASES,
            Name = "Cases",
            Description = "",
        };
        yield return new ContentSharingGroup
        {
            Id = Constants.OWNER_DOCUMENTATION,
            Name = "Documentation",
            Description = "",
        };
    }

    private static async IAsyncEnumerable<Collective> GetCollectives()
    {
        await Task.CompletedTask;
        yield return new Collective
        {
            Id = 72,
            Name = "Kerry and Niels"
        };
    }
    private static async IAsyncEnumerable<CollectiveUser> GetCollectiveUsers()
    {
        await Task.CompletedTask;
        yield return new CollectiveUser
        {
            CollectiveId = 72,
            UserId = 2
        };
        yield return new CollectiveUser
        {
            CollectiveId = 72,
            UserId = 3
        };
    }

    private static async IAsyncEnumerable<UserGroupUserRoleUser> GetUserGroupUserRoleUsers()
    {
        await Task.CompletedTask;
        yield return new UserGroupUserRoleUser
        {
            UserGroupId = 1,
            UserRoleId = 6,
            UserId = 1
        };
        yield return new UserGroupUserRoleUser
        {
            UserGroupId = 1,
            UserRoleId = 11,
            UserId = 2
        };
        yield return new UserGroupUserRoleUser
        {
            UserGroupId = 1,
            UserRoleId = 6,
            UserId = 3
        };
    }

    private async IAsyncEnumerable<AccessRolePrivilege> GetAccessRolePrivileges()
    {
        await foreach(var nodeType in NodeTypeMigrator.GetNodeTypes()) 
        {
            yield return new AccessRolePrivilege
            {
                AccessRoleId = 6,
                ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(nodeType.Id)
            };
            yield return new AccessRolePrivilege
            {
                AccessRoleId = 6,
                ActionId = await _deleteNodeActionIdReaderByNodeTypeId.ReadAsync(nodeType.Id)
            };
            yield return new AccessRolePrivilege
            {
                AccessRoleId = 6,
                ActionId = await _editNodeActionIdReaderByNodeTypeId.ReadAsync(nodeType.Id)
            };
        }
        yield return new AccessRolePrivilege
        {
            AccessRoleId = 4,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(35)
        };
        yield return new AccessRolePrivilege
        {
            AccessRoleId = 4,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(36)
        };
        yield return new AccessRolePrivilege
        {
            AccessRoleId = 4,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(37)
        };

        yield return new AccessRolePrivilege
        {
            AccessRoleId = 11,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(26),
        };

        yield return new AccessRolePrivilege
        {
            AccessRoleId = 11,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(29),
        };
        yield return new AccessRolePrivilege
        {
            AccessRoleId = 11,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(23),
        };
        yield return new AccessRolePrivilege
        {
            AccessRoleId = 11,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(24),
        };
        yield return new AccessRolePrivilege
        {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/articles")
        };
        yield return new AccessRolePrivilege
        {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/blogs")
        };
        yield return new AccessRolePrivilege
        {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/contact")
        };
    }

    protected override async Task MigrateImpl()
    {
        await AnonimousUserCreator.CreateAsync(_postgresConnection);
        await TenantCreator.CreateAsync(GetTenants(), _postgresConnection);
        await ContentSharingGroupCreator.CreateAsync(GetContentSharingGroups(), _postgresConnection);
        await UserRoleCreator.CreateAsync(GetUserRoles(), _postgresConnection);
        await UserCreator.CreateAsync(ReadUsers(), _postgresConnection);
        await CollectiveCreator.CreateAsync(GetCollectives(), _postgresConnection);
        await CollectiveUserCreator.CreateAsync(GetCollectiveUsers(), _postgresConnection);
        await UserGroupUserRoleUserCreator.CreateAsync(GetUserGroupUserRoleUsers(), _postgresConnection);
        await UserGroupUserRoleUserCreator.CreateAsync(ReadUsers().Select(x => new UserGroupUserRoleUser { UserGroupId = 1, UserRoleId = 4, UserId = (int)x.Id! }), _postgresConnection);
        await UserGroupUserRoleUserCreator.CreateAsync(ReadUsers().Select(x => new UserGroupUserRoleUser { UserGroupId = 1, UserRoleId = 12, UserId = (int)x.Id! }), _postgresConnection);
        await AccessRolePrivilegeCreator.CreateAsync(GetAccessRolePrivileges(), _postgresConnection);
    }
    private async IAsyncEnumerable<User> ReadUsers()
    {

        using var readCommand = MysqlConnection.CreateCommand();
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
                    END avatar,
                    u.status user_status_id
                FROM users u
                LEFT JOIN bio b ON b.uid = u.uid
                LEFT JOIN node n2 ON n2.nid = b.nid
                LEFT JOIN node_revisions nr ON nr.nid = n2.nid AND nr.vid = n2.vid
                LEFT JOIN content_type_uprofile ctb ON ctb.nid = n2.nid AND ctb.vid = n2.vid
                WHERE u.uid IN (
                    select
                        distinct
                        u.uid
                    from(
                        select
                        u.uid
                        FROM users u
                        WHERE (u.`status` = 1 OR u.login <> 0) and u.uid NOT IN (0, 965, 1233, 1655, 1745, 1780, 6197)
                    ) u
                )
                AND u.uid <> 72
                AND (b.nid is NULL OR b.nid IN (
                    SELECT 
                        MAX(b.nid)
                    FROM bio b
                    WHERE b.uid = u.uid
                ) )
                """;

        var reader = await readCommand.ExecuteReaderAsync();

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
                Avatar = avatar,
                UserStatusId = reader.GetInt32("user_status_id"),
            };
        }
        await reader.CloseAsync();
    }
}
