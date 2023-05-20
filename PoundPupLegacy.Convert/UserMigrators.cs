﻿namespace PoundPupLegacy.Convert;

internal sealed class UserMigrator(
    IDatabaseConnections databaseConnections,
    IAnonimousUserCreator anonimousUserCreator,
    IEntityCreator<SystemGroup> systemGroupCreator,
    IEntityCreator<AccessRole> accessRoleCreator,
    IEntityCreator<Tenant> tenantCreator,
    IEntityCreator<ContentSharingGroup> contentSharingGroupCreator,
    IEntityCreator<Subgroup> subgroupCreator,
    IEntityCreator<User> userCreator,
    IEntityCreator<Collective> collectiveCreator,
    IEntityCreator<CollectiveUser> collectiveUserCreator,
    IEntityCreator<UserGroupUserRoleUser> userGroupUserRoleUserCreator
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "users";
    private static async IAsyncEnumerable<AccessRole> GetAccessRoles()
    {
        await Task.CompletedTask;
        yield return new AccessRole {
            Id = Constants.PPL_MEMBER,
            Name = "Member",
            UserGroupId = Constants.PPL,
        };
        yield return new AccessRole {
            Id = Constants.PPL_EDITOR,
            Name = "Editor",
            UserGroupId = Constants.PPL,
        };
        yield return new AccessRole {
            Id = Constants.CPCT_MEMBER,
            Name = "Member",
            UserGroupId = Constants.CPCT,
        };
        yield return new AccessRole {
            Id = Constants.CPCT_EDITOR,
            Name = "Editor",
            UserGroupId = Constants.CPCT,
        };
        yield return new AccessRole {
            Id = Constants.ADULT_AFTERMATH_MEMBER,
            Name = "Member",
            UserGroupId = Constants.ADULT_AFTERMATH,
        };
        yield return new AccessRole {
            Id = Constants.ADULT_AFTERMATH_EDITOR,
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
            AccessRoleNotLoggedIn = new AccessRole {
                Id = Constants.PPL_EVERYONE,
                Name = "Everyone",
                UserGroupId = null,
            },
            AdministratorRole = new AdministratorRole {
                Id = Constants.PPL_ADMINISTRATOR,
                UserGroupId = null,
            },
            PublicationStatusIdDefault = 1,
            CountryIdDefault = null,
            Logo = "PPL",
            SubTitle = "exposing the dark side of adoption",
            FooterText = "<h2>Pound Pup Legacy</h2>",
            CssFile = "ppl.css",
            FrontPageText = """
            <h1>Pound Pup Legacy</h1>
            <p>
            Inspired by stories shared by birth parents, adoptive parents, and adult adoptees, PPL explores the dark side of adoption, and the consequences illegal and unethical actions have on future family-life and the well-being of those affected by adoption.
            </p>
            <p>
            Too many children are placed for the benefit of agencies and based on the demands of prospective adoptive parents.
            </p>
            Too many children are placed in inappropriate homes because the business interests of adoption agencies have higher priority than the safety of children.
            <p>
            Pound Pup Legacy documents and archives <a href="/cases">cases</a> where the child placement system did not work in the best interest of the child and we offer a platform for those who want to express their thoughts and feelings about the dark side of child adoption.
            </p>
            """
        };
        yield return new Tenant {
            Id = Constants.CPCT,
            DomainName = "cpctresearch.info",
            Name = "CPCT Research",
            Description = "",
            AccessRoleNotLoggedIn = new AccessRole {
                Id = Constants.CPCT_EVERYONE,
                Name = "Everyone",
                UserGroupId = null,
            },
            AdministratorRole = new AdministratorRole {
                Id = Constants.CPCT_ADMINISTRATOR,
                UserGroupId = null,
            },
            PublicationStatusIdDefault = 2,
            CountryIdDefault = null,
            Logo = "CPCT",
            CssFile = "cpct.css",
            SubTitle = null,
            FooterText = null,
            FrontPageText = null,
        };
    }

    private static async IAsyncEnumerable<ContentSharingGroup> GetContentSharingGroups()
    {
        await Task.CompletedTask;
        yield return new ContentSharingGroup {
            Id = Constants.OWNER_GEOGRAPHY,
            Name = "Geographical Entities",
            Description = "",
            AdministratorRole = new AdministratorRole {
                Id = Constants.GEOGRAPHY_ADMINISTRATOR,
                UserGroupId = null
            }
        };
        yield return new ContentSharingGroup {
            Id = Constants.OWNER_PARTIES,
            Name = "Parties",
            Description = "",
            AdministratorRole = new AdministratorRole {
                Id = Constants.PARTIES_ADMINISTRATOR,
                UserGroupId = null
            }
        };
        yield return new ContentSharingGroup {
            Id = Constants.OWNER_CASES,
            Name = "Cases",
            Description = "",
            AdministratorRole = new AdministratorRole {
                Id = Constants.CASES_ADMINISTRATOR,
                UserGroupId = null
            }
        };
        yield return new ContentSharingGroup {
            Id = Constants.OWNER_DOCUMENTATION,
            Name = "Documentation",
            Description = "",
            AdministratorRole = new AdministratorRole {
                Id = Constants.DOCUMENTATION_ADMINISTRATOR,
                UserGroupId = null
            }
        };
    }

    private static async IAsyncEnumerable<Collective> GetCollectives()
    {
        await Task.CompletedTask;
        yield return new Collective {
            Id = Constants.COLLECTIVE_NIELS_KERRY,
            Name = "Kerry and Niels"
        };
    }
    private static async IAsyncEnumerable<CollectiveUser> GetCollectiveUsers()
    {
        await Task.CompletedTask;
        yield return new CollectiveUser {
            CollectiveId = Constants.COLLECTIVE_NIELS_KERRY,
            UserId = Constants.USER_NIELS
        };
        yield return new CollectiveUser {
            CollectiveId = Constants.COLLECTIVE_NIELS_KERRY,
            UserId = Constants.USER_KERRY
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
            AdministratorRole = new AdministratorRole { Id = Constants.ADULT_AFTERMATH_ADMINSTRATOR, UserGroupId = null },
            PublicationStatusIdDefault = 2
        };
    }

    private static async IAsyncEnumerable<UserGroupUserRoleUser> GetUserGroupUserRoleUsers()
    {
        await Task.CompletedTask;
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.OWNER_SYSTEM,
            UserRoleId = Constants.SYSTEM_ADMINISTRATOR,
            UserId = Constants.USER_ADMINISTRATOR
        };

        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.PPL,
            UserRoleId = Constants.PPL_ADMINISTRATOR,
            UserId = Constants.USER_ADMINISTRATOR
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.PPL,
            UserRoleId = Constants.PPL_ADMINISTRATOR,
            UserId = Constants.USER_KERRY
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.PPL,
            UserRoleId = Constants.PPL_EDITOR,
            UserId = Constants.USER_NIELS
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.CPCT,
            UserRoleId = Constants.CPCT_ADMINISTRATOR,
            UserId = Constants.USER_ADMINISTRATOR
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.CPCT,
            UserRoleId = Constants.CPCT_ADMINISTRATOR,
            UserId = Constants.USER_ROELIE
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.CPCT,
            UserRoleId = Constants.CPCT_EDITOR,
            UserId = Constants.USER_NIELS
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.OWNER_GEOGRAPHY,
            UserRoleId = Constants.GEOGRAPHY_ADMINISTRATOR,
            UserId = Constants.USER_ADMINISTRATOR
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.OWNER_PARTIES,
            UserRoleId = Constants.PARTIES_ADMINISTRATOR,
            UserId = Constants.USER_ADMINISTRATOR
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.OWNER_CASES,
            UserRoleId = Constants.CASES_ADMINISTRATOR,
            UserId = Constants.USER_ADMINISTRATOR
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.OWNER_DOCUMENTATION,
            UserRoleId = Constants.DOCUMENTATION_ADMINISTRATOR,
            UserId = Constants.USER_ADMINISTRATOR
        };
        yield return new UserGroupUserRoleUser {
            UserGroupId = Constants.ADULT_AFTERMATH,
            UserRoleId = Constants.ADULT_AFTERMATH_ADMINSTRATOR,
            UserId = Constants.USER_ADMINISTRATOR
        };
    }


    protected override async Task MigrateImpl()
    {
        await anonimousUserCreator.CreateAsync(_postgresConnection);
        await tenantCreator.CreateAsync(GetTenants(), _postgresConnection);
        await userCreator.CreateAsync(ReadUsers(), _postgresConnection);
        await systemGroupCreator.CreateAsync(new SystemGroup {
            VocabularyTagging = new Vocabulary {
                Id = null,
                Name = Constants.VOCABULARY_TOPICS,
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = Constants.VOCABULARY_TOPICS,
                OwnerId = Constants.OWNER_SYSTEM,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode>()
            {
                new TenantNode
                {
                    Id = null,
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.TOPICS
                },
                new TenantNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.TOPICS
                }

            },
                NodeTypeId = Constants.VOCABULARY,
                Description = ""
            }
        }, _postgresConnection);

        await contentSharingGroupCreator.CreateAsync(GetContentSharingGroups(), _postgresConnection);
        await subgroupCreator.CreateAsync(GetSubgroups(), _postgresConnection);
        await accessRoleCreator.CreateAsync(GetAccessRoles(), _postgresConnection);

        await collectiveCreator.CreateAsync(GetCollectives(), _postgresConnection);
        await collectiveUserCreator.CreateAsync(GetCollectiveUsers(), _postgresConnection);
        await userGroupUserRoleUserCreator.CreateAsync(GetUserGroupUserRoleUsers(), _postgresConnection);
        await userGroupUserRoleUserCreator.CreateAsync(ReadUsers().Select(x => new UserGroupUserRoleUser { UserGroupId = Constants.PPL, UserRoleId = Constants.PPL_MEMBER, UserId = (int)x.Id! }), _postgresConnection);
        await userGroupUserRoleUserCreator.CreateAsync(ReadUsers().Select(x => new UserGroupUserRoleUser { UserGroupId = Constants.PPL, UserRoleId = Constants.PPL_EVERYONE, UserId = (int)x.Id! }), _postgresConnection);
        await userGroupUserRoleUserCreator.CreateAsync(new List<int> { 137, 136, 135, 134, 131, 2, 1 }.Select(x => new UserGroupUserRoleUser { UserGroupId = Constants.CPCT, UserRoleId = Constants.CPCT_MEMBER, UserId = x }).ToAsyncEnumerable(), _postgresConnection);
        await userGroupUserRoleUserCreator.CreateAsync(ReadAdultAftermathMembers(), _postgresConnection);

    }
    private async IAsyncEnumerable<User
> ReadUsers()
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
