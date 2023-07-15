using PoundPupLegacy.DomainModel;
using PoundPupLegacy.DomainModel.Creators;
using PoundPupLegacy.DomainModel.Readers;

namespace PoundPupLegacy.Convert;

internal sealed class AccessRolePrivilegeMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<CreateNodeActionIdReaderByNodeTypeIdRequest, int> createNodeActionIdReaderByNodeTypeIdFactory,
    IMandatorySingleItemDatabaseReaderFactory<ActionIdReaderByPathRequest, int> actionIdReaderByPathFactory,
    IMandatorySingleItemDatabaseReaderFactory<EditNodeActionIdReaderByNodeTypeIdRequest, int> editNodeActionIdReaderByNodeTypeIdFactory,
    IMandatorySingleItemDatabaseReaderFactory<EditOwnNodeActionIdReaderByNodeTypeIdRequest, int> editOwnNodeActionIdReaderByNodeTypeIdFactory,
    IEntityCreatorFactory<AccessRolePrivilege> accessRolePrivilegeCreatorFactory
) : MigratorPPL(databaseConnections)
{

    protected override string Name => "users";
    private async IAsyncEnumerable<AccessRolePrivilege> GetAccessRolePrivileges(
        IMandatorySingleItemDatabaseReader<CreateNodeActionIdReaderByNodeTypeIdRequest, int> createNodeActionIdReaderByNodeTypeId,
        IMandatorySingleItemDatabaseReader<ActionIdReaderByPathRequest, int> actionIdReaderByPath,
        IMandatorySingleItemDatabaseReader<EditNodeActionIdReaderByNodeTypeIdRequest, int> editNodeActionIdReaderByNodeTypeId,
        IMandatorySingleItemDatabaseReader<EditOwnNodeActionIdReaderByNodeTypeIdRequest, int> editOwnNodeActionIdReaderByNodeTypeId)
    {

        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_MEMBER,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.BLOG_POST
            })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_MEMBER,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.DOCUMENT
            })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_MEMBER,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.DISCUSSION
            })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.ADULT_AFTERMATH_MEMBER,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.BLOG_POST
            })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.ADULT_AFTERMATH_MEMBER,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.DOCUMENT
            })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.ADULT_AFTERMATH_MEMBER,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.DISCUSSION
            })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.ORGANIZATION
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.PERSON
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.ABUSE_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.CHILD_TRAFFICKING_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.COERCED_ADOPTION_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.DEPORTATION_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.DISRUPTED_PLACEMENT_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.FATHERS_RIGHTS_VIOLATION_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.WRONGFUL_MEDICATION_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.WRONGFUL_REMOVAL_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.ORGANIZATION
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.PERSON
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.ABUSE_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.CHILD_TRAFFICKING_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.COERCED_ADOPTION_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.DEPORTATION_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.DISRUPTED_PLACEMENT_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.FATHERS_RIGHTS_VIOLATION_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.WRONGFUL_MEDICATION_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_EDITOR,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.WRONGFUL_REMOVAL_CASE
            }),
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/documents" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/blogs" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/organizations" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/persons" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/polls" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/abuse_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/child_trafficking_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/deportation_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/wrongful_removal_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/wrongful_medication_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/fathers_rights_violation_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/coerced_adoption_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/disrupted_placement_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/topics" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/countries" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/contact" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/search" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/node/{Id:int}" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/blog/{Id:int}" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/united_states_congress" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/united_states_senate/{Id:int}" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.PPL_EVERYONE,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/united_states_house_of_representatives/{Id:int}" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_MEMBER,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/organizations" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_MEMBER,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/persons" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_MEMBER,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_MEMBER,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/abuse_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_MEMBER,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/child_trafficking_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_MEMBER,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/deportation_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_MEMBER,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/wrongful_removal_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_MEMBER,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/wrongful_medication_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_MEMBER,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/fathers_rights_violation_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_MEMBER,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/coerced_adoption_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_MEMBER,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/disrupted_placement_cases" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_MEMBER,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/countries" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_MEMBER,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/search" })
        };
        yield return new AccessRolePrivilege {
            AccessRoleId = Constants.CPCT_MEMBER,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/node/{Id:int}" })
        };
        await foreach (var nodeType in NodeTypeMigrator.GetNodeTypes()) {
            yield return new AccessRolePrivilege {
                AccessRoleId = Constants.PPL_EDITOR,
                ActionId = await editNodeActionIdReaderByNodeTypeId.ReadAsync(new EditNodeActionIdReaderByNodeTypeIdRequest {
                    NodeTypeId = nodeType.Identification.Id!.Value
                })
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = Constants.CPCT_EDITOR,
                ActionId = await editNodeActionIdReaderByNodeTypeId.ReadAsync(new EditNodeActionIdReaderByNodeTypeIdRequest {
                    NodeTypeId = nodeType.Identification.Id!.Value
                })
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = Constants.ADULT_AFTERMATH_EDITOR,
                ActionId = await editNodeActionIdReaderByNodeTypeId.ReadAsync(new EditNodeActionIdReaderByNodeTypeIdRequest {
                    NodeTypeId = nodeType.Identification.Id!.Value
                })
            };
        }
        await foreach (var nodeType in NodeTypeMigrator.GetNodeTypes().Where(x => x.AuthorSpecific)) {
            yield return new AccessRolePrivilege {
                AccessRoleId = Constants.PPL_MEMBER,
                ActionId = await editOwnNodeActionIdReaderByNodeTypeId.ReadAsync(new EditOwnNodeActionIdReaderByNodeTypeIdRequest {
                    NodeTypeId = nodeType.Identification.Id!.Value
                })
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = Constants.CPCT_MEMBER,
                ActionId = await editOwnNodeActionIdReaderByNodeTypeId.ReadAsync(new EditOwnNodeActionIdReaderByNodeTypeIdRequest {
                    NodeTypeId = nodeType.Identification.Id!.Value
                })
            };
            yield return new AccessRolePrivilege {
                AccessRoleId = Constants.ADULT_AFTERMATH_MEMBER,
                ActionId = await editOwnNodeActionIdReaderByNodeTypeId.ReadAsync(new EditOwnNodeActionIdReaderByNodeTypeIdRequest {
                    NodeTypeId = nodeType.Identification.Id!.Value
                })
            };
        }
    }

    protected override async Task MigrateImpl()
    {
        await using var createNodeActionIdReaderByNodeTypeId = await createNodeActionIdReaderByNodeTypeIdFactory.CreateAsync(_postgresConnection);
        await using var actionIdReaderByPath = await actionIdReaderByPathFactory.CreateAsync(_postgresConnection);
        await using var editNodeActionIdReaderByNodeTypeId = await editNodeActionIdReaderByNodeTypeIdFactory.CreateAsync(_postgresConnection);
        await using var editOwnNodeActionIdReaderByNodeTypeId = await editOwnNodeActionIdReaderByNodeTypeIdFactory.CreateAsync(_postgresConnection);
        await using var accessRolePrivilegeCreator = await accessRolePrivilegeCreatorFactory.CreateAsync(_postgresConnection);
        await accessRolePrivilegeCreator.CreateAsync(GetAccessRolePrivileges(
            createNodeActionIdReaderByNodeTypeId,
            actionIdReaderByPath,
            editNodeActionIdReaderByNodeTypeId,
            editOwnNodeActionIdReaderByNodeTypeId
        ));
    }
}
