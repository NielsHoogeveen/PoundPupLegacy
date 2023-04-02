namespace PoundPupLegacy.Convert;

internal sealed class UserMigrator : MigratorPPL
{
    protected override string Name => "users";
    private readonly IAnonimousUserCreator _anonimousUserCreator;
    private readonly ISystemGroupCreator _systemGroupCreator;
    private readonly IEntityCreator<AccessRole> _accessRoleCreator;
    private readonly IEntityCreator<Tenant> _tenantCreator;
    private readonly IEntityCreator<ContentSharingGroup> _contentSharingGroupCreator;
    private readonly IEntityCreator<Subgroup> _subgroupCreator;
    private readonly IEntityCreator<User> _userCreator;
    private readonly IEntityCreator<Collective> _collectiveCreator;
    private readonly IEntityCreator<CollectiveUser> _collectiveUserCreator;
    private readonly IEntityCreator<UserGroupUserRoleUser> _userGroupUserRoleUserCreator;

    public UserMigrator(
        IDatabaseConnections databaseConnections,
        IAnonimousUserCreator anonimousUserCreator,
        ISystemGroupCreator systemGroupCreator,
        IEntityCreator<AccessRole> accessRoleCreator,
        IEntityCreator<Tenant> tenantCreator,
        IEntityCreator<ContentSharingGroup> contentSharingGroupCreator,
        IEntityCreator<Subgroup> subgroupCreator,
        IEntityCreator<User> userCreator,
        IEntityCreator<Collective> collectiveCreator,
        IEntityCreator<CollectiveUser> collectiveUserCreator,
        IEntityCreator<UserGroupUserRoleUser> userGroupUserRoleUserCreator
    ) : base(databaseConnections)
    {
        _anonimousUserCreator = anonimousUserCreator;
        _systemGroupCreator = systemGroupCreator;
        _accessRoleCreator = accessRoleCreator;
        _tenantCreator = tenantCreator;
        _contentSharingGroupCreator = contentSharingGroupCreator;
        _subgroupCreator = subgroupCreator;
        _userCreator = userCreator;
        _collectiveCreator = collectiveCreator;
        _collectiveUserCreator = collectiveUserCreator;
        _userGroupUserRoleUserCreator = userGroupUserRoleUserCreator;
    }

    private static async IAsyncEnumerable<AccessRole> GetAccessRoles()
    {
        await Task.CompletedTask;
        yield return new AccessRole {
            Id = 4,
            Name = "Member",
            UserGroupId = Constants.PPL,
        };
        yield return new AccessRole {
            Id = 11,
            Name = "Editor",
            UserGroupId = Constants.PPL,
        };
        yield return new AccessRole {
            Id = 16,
            Name = "Member",
            UserGroupId = Constants.CPCT,
        };
        yield return new AccessRole {
            Id = 18,
            Name = "Editor",
            UserGroupId = Constants.CPCT,
        };
        yield return new AccessRole {
            Id = 42,
            Name = "Member",
            UserGroupId = Constants.ADULT_AFTERMATH,
        };
        yield return new AccessRole {
            Id = 43,
            Name = "Editor",
            UserGroupId = Constants.ADULT_AFTERMATH,
        };
    }
    private static async IAsyncEnumerable<Tenant> GetTenants()
    {
        await Task.CompletedTask;
        yield return new Tenant {
            Id = Constants.PPL,
            DomainName = "poundpuplegacy.org",
            Name = "Pound Pup Legacy",
            Description = "",
            VocabularyIdTagging = null,
            AccessRoleNotLoggedIn = new AccessRole {
                Id = 12,
                Name = "Everyone",
                UserGroupId = null,
            },
            AdministratorRole = new AdministratorRole {
                Id = 6,
                UserGroupId = null,
            },
            PublicationStatusIdDefault = 1,
            CountryIdDefault = null,
        };
        yield return new Tenant {
            Id = Constants.CPCT,
            DomainName = "cpctresearch.info",
            Name = "CPCT Research",
            Description = "",
            VocabularyIdTagging = null,
            AccessRoleNotLoggedIn = new AccessRole {
                Id = 13,
                Name = "Everyone",
                UserGroupId = null,
            },
            AdministratorRole = new AdministratorRole {
                Id = 19,
                UserGroupId = null,
            },
            PublicationStatusIdDefault = 2,
            CountryIdDefault = null,
        };
    }

    private static async IAsyncEnumerable<ContentSharingGroup> GetContentSharingGroups()
    {
        await Task.CompletedTask;
        yield return new ContentSharingGroup {
            Id = Constants.OWNER_GEOGRAPHY,
            Name = "Geographical Entities",
            Description = "",
            AdministratorRole = new AdministratorRole { Id = 29, UserGroupId = null }
        };
        yield return new ContentSharingGroup {
            Id = Constants.OWNER_PARTIES,
            Name = "Parties",
            Description = "",
            AdministratorRole = new AdministratorRole { Id = 31, UserGroupId = null }
        };
        yield return new ContentSharingGroup {
            Id = Constants.OWNER_CASES,
            Name = "Cases",
            Description = "",
            AdministratorRole = new AdministratorRole { Id = 33, UserGroupId = null }
        };
        yield return new ContentSharingGroup {
            Id = Constants.OWNER_DOCUMENTATION,
            Name = "Documentation",
            Description = "",
            AdministratorRole = new AdministratorRole { Id = 34, UserGroupId = null }
        };
    }

    private static async IAsyncEnumerable<Collective> GetCollectives()
    {
        await Task.CompletedTask;
        yield return new Collective {
            Id = 72,
            Name = "Kerry and Niels"
        };
    }
    private static async IAsyncEnumerable<CollectiveUser> GetCollectiveUsers()
    {
        await Task.CompletedTask;
        yield return new CollectiveUser {
            CollectiveId = 72,
            UserId = 2
        };
        yield return new CollectiveUser {
            CollectiveId = 72,
            UserId = 3
        };
    }

    private static async IAsyncEnumerable<Subgroup> GetSubgroups()
    {
        await Task.CompletedTask;
        yield return new Subgroup {
            Id = Constants.ADULT_AFTERMATH,
            Name = "Adult Aftermath",
            Description = "Group for private discusssions",
            TenantId = Constants.PPL,
            AdministratorRole = new AdministratorRole { Id = 41, UserGroupId = null },
            PublicationStatusIdDefault = 2
        };
    }

    private static async IAsyncEnumerable<UserGroupUserRoleUser> GetUserGroupUserRoleUsers()
    {
        await Task.CompletedTask;
        yield return new UserGroupUserRoleUser {
            UserGroupId = 0,
            UserRoleId = 21,
            UserId = 1
        };

        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.PPL,
            UserRoleId = 6,
            UserId = 1
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.PPL,
            UserRoleId = 6,
            UserId = 3
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.PPL,
            UserRoleId = 11,
            UserId = 2
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.CPCT,
            UserRoleId = 19,
            UserId = 1
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.CPCT,
            UserRoleId = 19,
            UserId = 131
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.CPCT,
            UserRoleId = 18,
            UserId = 2
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.OWNER_GEOGRAPHY,
            UserRoleId = 29,
            UserId = 1
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.OWNER_PARTIES,
            UserRoleId = 31,
            UserId = 1
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.OWNER_CASES,
            UserRoleId = 33,
            UserId = 1
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.OWNER_DOCUMENTATION,
            UserRoleId = 34,
            UserId = 1
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.ADULT_AFTERMATH,
            UserRoleId = 41,
            UserId = 1
        };
    }


    protected override async Task MigrateImpl()
    {
        await _anonimousUserCreator.CreateAsync(_postgresConnection);
        await _systemGroupCreator.CreateAsync(_postgresConnection);
        await _tenantCreator.CreateAsync(GetTenants(), _postgresConnection);
        await _contentSharingGroupCreator.CreateAsync(GetContentSharingGroups(), _postgresConnection);
        await _subgroupCreator.CreateAsync(GetSubgroups(), _postgresConnection);
        await _accessRoleCreator.CreateAsync(GetAccessRoles(), _postgresConnection);
        await _userCreator.CreateAsync(ReadUsers(), _postgresConnection);
        await _collectiveCreator.CreateAsync(GetCollectives(), _postgresConnection);
        await _collectiveUserCreator.CreateAsync(GetCollectiveUsers(), _postgresConnection);
        await _userGroupUserRoleUserCreator.CreateAsync(GetUserGroupUserRoleUsers(), _postgresConnection);
        await _userGroupUserRoleUserCreator.CreateAsync(ReadUsers().Select(x => new UserGroupUserRoleUser { UserGroupId = 1, UserRoleId = 4, UserId = (int)x.Id! }), _postgresConnection);
        await _userGroupUserRoleUserCreator.CreateAsync(ReadUsers().Select(x => new UserGroupUserRoleUser { UserGroupId = 1, UserRoleId = 12, UserId = (int)x.Id! }), _postgresConnection);
        await _userGroupUserRoleUserCreator.CreateAsync(new List<int> { 137, 136, 135, 134, 131, 2, 1 }.Select(x => new UserGroupUserRoleUser { UserGroupId = 6, UserRoleId = 16, UserId = x }).ToAsyncEnumerable(), _postgresConnection);
        await _userGroupUserRoleUserCreator.CreateAsync(ReadAdultAftermathMembers(), _postgresConnection);

    }
    private async IAsyncEnumerable<User> ReadUsers()
    {

        using var readCommand = _mySqlConnection.CreateCommand();
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

        while (reader.Read()) {
            var aboutMe = reader.IsDBNull("about_me") ? null : reader.GetString("about_me") == "" ? null : reader.GetString("about_me");
            var animalWithing = reader.IsDBNull("animal_within") ? null : reader.GetString("animal_within") == "" ? null : reader.GetString("animal_within");
            var relationToChildPlacement = reader.IsDBNull("relation_to_child_placement") ? "Other" : reader.GetString("relation_to_child_placement");
            var avatar = reader.IsDBNull("avatar") ? null : reader.GetString("avatar") == "" ? null : reader.GetString("avatar");
            yield return new User {
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

    private async IAsyncEnumerable<UserGroupUserRoleUser> ReadAdultAftermathMembers()
    {

        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = """
                SELECT
                og_uid.uid uid,
                case 
                	when og_uid.is_admin = 1 then 41
                	ELSE 42
                END access_role_id
                FROM og
                JOIN og_uid ON og_uid.nid = og.nid
                WHERE og.nid = 17146
                """;

        var reader = await readCommand.ExecuteReaderAsync();

        while (reader.Read()) {
            yield return new UserGroupUserRoleUser {
                UserGroupId = Constants.ADULT_AFTERMATH,
                UserId = reader.GetInt32("uid"),
                UserRoleId = reader.GetInt32("access_role_id")
            };
        }
        await reader.CloseAsync();
    }

}
