namespace PoundPupLegacy.Convert;

internal sealed class AccessRolePrivilegeMigrator : MigratorPPL
{
    private readonly IMandatorySingleItemDatabaseReaderFactory<CreateNodeActionIdReaderByNodeTypeIdRequest, int> _createNodeActionIdReaderByNodeTypeIdFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<ActionIdReaderByPathRequest, int> _actionIdReaderByPathFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<EditNodeActionIdReaderByNodeTypeIdRequest, int> _editNodeActionIdReaderByNodeTypeIdFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<EditOwnNodeActionIdReaderByNodeTypeIdRequest, int> _editOwnNodeActionIdReaderByNodeTypeIdFactory;
    private readonly IEntityCreator<AccessRolePrivilege> _accessRolePrivilegeCreator;

    protected override string Name => "users";
    public AccessRolePrivilegeMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<CreateNodeActionIdReaderByNodeTypeIdRequest, int> createNodeActionIdReaderByNodeTypeIdFactory,
        IMandatorySingleItemDatabaseReaderFactory<ActionIdReaderByPathRequest, int> actionIdReaderByPathFactory,
        IMandatorySingleItemDatabaseReaderFactory<EditNodeActionIdReaderByNodeTypeIdRequest, int> editNodeActionIdReaderByNodeTypeIdFactory,
        IMandatorySingleItemDatabaseReaderFactory<EditOwnNodeActionIdReaderByNodeTypeIdRequest, int> editOwnNodeActionIdReaderByNodeTypeIdFactory,
        IEntityCreator<AccessRolePrivilege> accessRolePrivilegeCreator

    )
    : base(databaseConnections)
    {
        _createNodeActionIdReaderByNodeTypeIdFactory = createNodeActionIdReaderByNodeTypeIdFactory;
        _actionIdReaderByPathFactory = actionIdReaderByPathFactory;
        _editNodeActionIdReaderByNodeTypeIdFactory = editNodeActionIdReaderByNodeTypeIdFactory;
        _editOwnNodeActionIdReaderByNodeTypeIdFactory = editOwnNodeActionIdReaderByNodeTypeIdFactory;
        _accessRolePrivilegeCreator = accessRolePrivilegeCreator;
    }

    private async IAsyncEnumerable<AccessRolePrivilege> GetAccessRolePrivileges(
        IMandatorySingleItemDatabaseReader<CreateNodeActionIdReaderByNodeTypeIdRequest, int> createNodeActionIdReaderByNodeTypeId,
        IMandatorySingleItemDatabaseReader<ActionIdReaderByPathRequest, int> actionIdReaderByPath,
        IMandatorySingleItemDatabaseReader<EditNodeActionIdReaderByNodeTypeIdRequest, int> editNodeActionIdReaderByNodeTypeId,
        IMandatorySingleItemDatabaseReader<EditOwnNodeActionIdReaderByNodeTypeIdRequest, int> editOwnNodeActionIdReaderByNodeTypeId)
    {

        yield return new AccessRolePrivilege {
            AccessRoleId = 4,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = 35
            })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 4,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = 36
            })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 4,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = 37
            })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 42,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = 35
            })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 42,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = 36
            })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 42,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = 37
            })
        };

        yield return new AccessRolePrivilege {
            AccessRoleId = 11,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = 26
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 11,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = 29
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 11,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = 23
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 11,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = 24
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 18,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = 23
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 18,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = 24
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/articles" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/blogs" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/organizations" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/persons" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/polls" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/abuse_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/child_trafficking_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/deportation_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/wrongful_removal_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/wrongful_medication_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/fathers_rights_violation_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/coerced_adoption_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/disrupted_placement_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/topics" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/countries" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/contact" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/search" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/node/{Id:int}" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/blog/{Id:int}" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/united_states_congress" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/united_states_senate/{Id:int}" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 12,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/united_states_house_of_representatives/{Id:int}" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/organizations" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/persons" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/abuse_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/child_trafficking_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/deportation_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/wrongful_removal_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/wrongful_medication_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/fathers_rights_violation_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/coerced_adoption_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/disrupted_placement_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/countries" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/search" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = 16,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/node/{Id:int}" })
        };
        await foreach (var nodeType in NodeTypeMigrator.GetNodeTypes()) {
            yield return new AccessRolePrivilege {
                AccessRoleId = 11,
                ActionId = await editNodeActionIdReaderByNodeTypeId.ReadAsync(new EditNodeActionIdReaderByNodeTypeIdRequest {
                    NodeTypeId = nodeType.Id!.Value
                })
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = 18,
                ActionId = await editNodeActionIdReaderByNodeTypeId.ReadAsync(new EditNodeActionIdReaderByNodeTypeIdRequest {
                    NodeTypeId = nodeType.Id!.Value
                })
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = 43,
                ActionId = await editNodeActionIdReaderByNodeTypeId.ReadAsync(new EditNodeActionIdReaderByNodeTypeIdRequest {
                    NodeTypeId = nodeType.Id!.Value
                })
            };
        }
        await foreach (var nodeType in NodeTypeMigrator.GetNodeTypes().Where(x => x.AuthorSpecific)) {
            yield return new AccessRolePrivilege {
                AccessRoleId = 4,
                ActionId = await editOwnNodeActionIdReaderByNodeTypeId.ReadAsync(new EditOwnNodeActionIdReaderByNodeTypeIdRequest {
                    NodeTypeId = nodeType.Id!.Value
                })
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = 16,
                ActionId = await editOwnNodeActionIdReaderByNodeTypeId.ReadAsync(new EditOwnNodeActionIdReaderByNodeTypeIdRequest {
                    NodeTypeId = nodeType.Id!.Value
                })
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = 42,
                ActionId = await editOwnNodeActionIdReaderByNodeTypeId.ReadAsync(new EditOwnNodeActionIdReaderByNodeTypeIdRequest {
                    NodeTypeId = nodeType.Id!.Value
                })
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
