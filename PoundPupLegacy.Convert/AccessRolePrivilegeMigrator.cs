using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal sealed class AccessRolePrivilegeMigrator : PPLMigrator
{
    protected override string Name => "users";
    public AccessRolePrivilegeMigrator(MySqlToPostgresConverter converter) : base(converter)
    {

    }

    private async IAsyncEnumerable<AccessRolePrivilege> GetAccessRolePrivileges()
    {

        yield return new AccessRolePrivilege {
            AccessRoleId = 4,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(35)
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 4,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(36)
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 4,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(37)
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 42,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(35)
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 42,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(36)
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 42,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(37)
        };

        yield return new AccessRolePrivilege {
            AccessRoleId = 11,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(26),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 11,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(29),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 11,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(23),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 11,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(24),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 18,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(23),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 18,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(24),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/articles")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/blogs")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/organizations")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/persons")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/polls")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/abuse_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/child_trafficking_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/deportation_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/wrongful_removal_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/wrongful_medication_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/fathers_rights_violation_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/coerced_adoption_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/disrupted_placement_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/topics")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/countries")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/contact")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await _actionReaderByPath.ReadAsync("/search")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await _actionReaderByPath.ReadAsync("/organizations")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await _actionReaderByPath.ReadAsync("/persons")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await _actionReaderByPath.ReadAsync("/countries")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await _actionReaderByPath.ReadAsync("/search")
        };
        await foreach (var nodeType in NodeTypeMigrator.GetNodeTypes()) {
            yield return new AccessRolePrivilege {
                AccessRoleId = 11,
                ActionId = await _editNodeActionIdReaderByNodeTypeId.ReadAsync(nodeType.Id!.Value)
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = 18,
                ActionId = await _editNodeActionIdReaderByNodeTypeId.ReadAsync(nodeType.Id!.Value)
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = 43,
                ActionId = await _editNodeActionIdReaderByNodeTypeId.ReadAsync(nodeType.Id!.Value)
            };
        }
        await foreach (var nodeType in NodeTypeMigrator.GetNodeTypes().Where(x => x.AuthorSpecific)) {
            yield return new AccessRolePrivilege {
                AccessRoleId = 4,
                ActionId = await _editOwnNodeActionIdReaderByNodeTypeId.ReadAsync(nodeType.Id!.Value)
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = 16,
                ActionId = await _editOwnNodeActionIdReaderByNodeTypeId.ReadAsync(nodeType.Id!.Value)
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = 42,
                ActionId = await _editOwnNodeActionIdReaderByNodeTypeId.ReadAsync(nodeType.Id!.Value)
            };
        }
    }

    protected override async Task MigrateImpl()
    {

        await AccessRolePrivilegeCreator.CreateAsync(GetAccessRolePrivileges(), _postgresConnection);
    }
}
