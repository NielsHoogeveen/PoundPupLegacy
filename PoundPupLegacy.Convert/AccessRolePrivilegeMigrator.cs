namespace PoundPupLegacy.Convert;

internal sealed class AccessRolePrivilegeMigrator : MigratorPPL
{
    private readonly IDatabaseReaderFactory<NodeIdReaderByUrlId> _nodeIdReaderFactory;
    private readonly IDatabaseReaderFactory<CreateNodeActionIdReaderByNodeTypeId> _createNodeActionIdReaderByNodeTypeIdFactory;
    private readonly IDatabaseReaderFactory<ActionIdReaderByPath> _actionIdReaderByPathFactory;
    private readonly IDatabaseReaderFactory<EditNodeActionIdReaderByNodeTypeId> _editNodeActionIdReaderByNodeTypeIdFactory;
    private readonly IDatabaseReaderFactory<EditOwnNodeActionIdReaderByNodeTypeId> _editOwnNodeActionIdReaderByNodeTypeIdFactory;
    private readonly IEntityCreator<AccessRolePrivilege> _accessRolePrivilegeCreator;

    protected override string Name => "users";
    public AccessRolePrivilegeMigrator(
        IDatabaseConnections databaseConnections,
        IDatabaseReaderFactory<NodeIdReaderByUrlId> nodeIdReaderFactory,
        IDatabaseReaderFactory<CreateNodeActionIdReaderByNodeTypeId> createNodeActionIdReaderByNodeTypeIdFactory,
        IDatabaseReaderFactory<ActionIdReaderByPath> actionIdReaderByPathFactory,
        IDatabaseReaderFactory<EditNodeActionIdReaderByNodeTypeId> editNodeActionIdReaderByNodeTypeIdFactory,
        IDatabaseReaderFactory<EditOwnNodeActionIdReaderByNodeTypeId> editOwnNodeActionIdReaderByNodeTypeIdFactory,
        IEntityCreator<AccessRolePrivilege> accessRolePrivilegeCreator

    )
    : base(databaseConnections)
    {
        _nodeIdReaderFactory = nodeIdReaderFactory;
        _createNodeActionIdReaderByNodeTypeIdFactory = createNodeActionIdReaderByNodeTypeIdFactory;
        _actionIdReaderByPathFactory = actionIdReaderByPathFactory;
        _editNodeActionIdReaderByNodeTypeIdFactory = editNodeActionIdReaderByNodeTypeIdFactory;
        _editOwnNodeActionIdReaderByNodeTypeIdFactory = editOwnNodeActionIdReaderByNodeTypeIdFactory;
        _accessRolePrivilegeCreator = accessRolePrivilegeCreator;
    }

    private async IAsyncEnumerable<AccessRolePrivilege> GetAccessRolePrivileges(
        CreateNodeActionIdReaderByNodeTypeId createNodeActionIdReaderByNodeTypeId,
        ActionIdReaderByPath actionIdReaderByPath,
        EditNodeActionIdReaderByNodeTypeId editNodeActionIdReaderByNodeTypeId,
        EditOwnNodeActionIdReaderByNodeTypeId editOwnNodeActionIdReaderByNodeTypeId)
    {

        yield return new AccessRolePrivilege {
            AccessRoleId = 4,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(35)
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 4,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(36)
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 4,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(37)
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 42,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(35)
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 42,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(36)
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 42,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(37)
        };

        yield return new AccessRolePrivilege {
            AccessRoleId = 11,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(26),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 11,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(29),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 11,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(23),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 11,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(24),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 18,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(23),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 18,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(24),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/articles")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/blogs")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/organizations")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/persons")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/polls")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/abuse_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/child_trafficking_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/deportation_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/wrongful_removal_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/wrongful_medication_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/fathers_rights_violation_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/coerced_adoption_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/disrupted_placement_cases")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/topics")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/countries")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/contact")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync("/search")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync("/organizations")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync("/persons")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync("/countries")
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync("/search")
        };
        await foreach (var nodeType in NodeTypeMigrator.GetNodeTypes()) {
            yield return new AccessRolePrivilege {
                AccessRoleId = 11,
                ActionId = await editNodeActionIdReaderByNodeTypeId.ReadAsync(nodeType.Id!.Value)
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = 18,
                ActionId = await editNodeActionIdReaderByNodeTypeId.ReadAsync(nodeType.Id!.Value)
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = 43,
                ActionId = await editNodeActionIdReaderByNodeTypeId.ReadAsync(nodeType.Id!.Value)
            };
        }
        await foreach (var nodeType in NodeTypeMigrator.GetNodeTypes().Where(x => x.AuthorSpecific)) {
            yield return new AccessRolePrivilege {
                AccessRoleId = 4,
                ActionId = await editOwnNodeActionIdReaderByNodeTypeId.ReadAsync(nodeType.Id!.Value)
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = 16,
                ActionId = await editOwnNodeActionIdReaderByNodeTypeId.ReadAsync(nodeType.Id!.Value)
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = 42,
                ActionId = await editOwnNodeActionIdReaderByNodeTypeId.ReadAsync(nodeType.Id!.Value)
            };
        }
    }

    protected override async Task MigrateImpl()
    {
        await using var createNodeActionIdReaderByNodeTypeId = await _createNodeActionIdReaderByNodeTypeIdFactory.CreateAsync(_postgresConnection);
        await using var actionIdReaderByPath = await _actionIdReaderByPathFactory.CreateAsync(_postgresConnection);
        await using var editNodeActionIdReaderByNodeTypeId = await _editNodeActionIdReaderByNodeTypeIdFactory.CreateAsync(_postgresConnection);
        await using var editOwnNodeActionIdReaderByNodeTypeId = await _editOwnNodeActionIdReaderByNodeTypeIdFactory.CreateAsync(_postgresConnection);

        await _accessRolePrivilegeCreator.CreateAsync(GetAccessRolePrivileges(
            createNodeActionIdReaderByNodeTypeId,
            actionIdReaderByPath,
            editNodeActionIdReaderByNodeTypeId,
            editOwnNodeActionIdReaderByNodeTypeId
        ), _postgresConnection);
    }
}
