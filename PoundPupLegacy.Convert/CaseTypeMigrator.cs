namespace PoundPupLegacy.Convert;

internal sealed class CaseTypeMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<ActionIdReaderByPathRequest, int> actionIdReaderFactory,
        IInsertingEntityCreatorFactory<ViewNodeTypeListAction> viewNodeTypeListActionCreatorFactory,
        IInsertingEntityCreatorFactory<CaseType> caseTypeCreatorFactory,
        IInsertingEntityCreatorFactory<CreateNodeAction> createNodeActionCreatorFactory,
        IInsertingEntityCreatorFactory<DeleteNodeAction> deleteNodeActionCreatorFactory,
        IInsertingEntityCreatorFactory<EditNodeAction> editNodeActionCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "node types";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var actionIdReader = await actionIdReaderFactory.CreateAsync(_postgresConnection);
        await using var caseTypeCreator = await caseTypeCreatorFactory.CreateAsync(_postgresConnection);
        await using var viewNodeTypeListActionCreator = await viewNodeTypeListActionCreatorFactory.CreateAsync(_postgresConnection);
        await using var createNodeActionCreator = await createNodeActionCreatorFactory.CreateAsync(_postgresConnection);
        await using var deleteNodeActionCreator = await deleteNodeActionCreatorFactory.CreateAsync(_postgresConnection);
        await using var editNodeActionCreator = await editNodeActionCreatorFactory.CreateAsync(_postgresConnection);
        await caseTypeCreator.CreateAsync(GetCaseTypes(nodeIdReader));
        await viewNodeTypeListActionCreator.CreateAsync(GetViewNodeTypeListActions(actionIdReader));
        await createNodeActionCreator.CreateAsync(GetCaseTypes(nodeIdReader).Select(x => new CreateNodeAction { Id = null, NodeTypeId = x.Id!.Value }));
        await deleteNodeActionCreator.CreateAsync(GetCaseTypes(nodeIdReader).Select(x => new DeleteNodeAction { Id = null, NodeTypeId = x.Id!.Value }));
        await editNodeActionCreator.CreateAsync(GetCaseTypes(nodeIdReader).Select(x => new EditNodeAction { Id = null, NodeTypeId = x.Id!.Value }));
    }
    internal async IAsyncEnumerable<CaseType> GetCaseTypes(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {

        yield return new CaseType {
            Id = Constants.ABUSE_CASE,
            AuthorSpecific = false,
            Name = "abuse case",
            Description = "Abuse case of a child that has been placed by court",
            TagLabelName = "Cases",
            Text = "The assumption behind child-placement is that the safety and living conditions of a child improve. These cases demonstrate that this assumption is often invalid.",
            CaseRelationTypeIds = new List<int>
            {
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.HOMESTUDY_CASE_TYPE
                }),
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.PLACEMENT_CASE_TYPE
                }),
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.POSTPLACEMENT_CASE_TYPE
                }),
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.FACILITATION_CASE_TYPE
                }),
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.INSTITUTION_CASE_TYPE
                }),
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.THERAPY_CASE_TYPE
                })
            }
        };
        yield return new CaseType {
            Id = Constants.CHILD_TRAFFICKING_CASE,
            AuthorSpecific = false,
            Name = "child trafficking case",
            Description = "Trafficking case of children to be adopted",
            TagLabelName = "Cases",
            Text = "There is often a fine line between adoption and child trafficking. In many cases this line is being crossed.",
            CaseRelationTypeIds = new List<int>
            {
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.PLACEMENT_CASE_TYPE
                }),
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.FACILITATION_CASE_TYPE
                }),
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.INSTITUTION_CASE_TYPE
                })
            }
        };
        yield return new CaseType {
            Id = Constants.COERCED_ADOPTION_CASE,
            AuthorSpecific = false,
            Name = "coerced adoption case",
            Description = "Adoption that involved coercion",
            TagLabelName = "Cases",
            Text = "Adoption is assumed to be the result of a choice made by the parents of the child. These cases demonstrate women are pressured to give up their children.",
            CaseRelationTypeIds = new List<int>
            {
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.PLACEMENT_CASE_TYPE
                }),
            }
        };
        yield return new CaseType {
            Id = Constants.DEPORTATION_CASE,
            AuthorSpecific = false,
            Name = "deportation case",
            Description = "Adoptees deported to country of origin",
            TagLabelName = "Cases",
            Text = "Adoptions before 1997, didn't automatically lead to naturalization. As result, people adopted from outside the outside US that ran into problems with the justice system face deportation to their country of birth.",
            CaseRelationTypeIds = new List<int>()
        };
        yield return new CaseType {
            Id = Constants.FATHERS_RIGHTS_VIOLATION_CASE,
            AuthorSpecific = false,
            Name = "father's rights violation case",
            Description = "Adoptions where the rights of the biological father were violated",
            TagLabelName = "Cases",
            Text = "Adoption requires the consent of both biological parents. These cases demonstrate that the rights of fathers in adoption cases are being violated.",
            CaseRelationTypeIds = new List<int>
            {
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.PLACEMENT_CASE_TYPE
                }),
            }
        };
        yield return new CaseType {
            Id = Constants.WRONGFUL_MEDICATION_CASE,
            AuthorSpecific = false,
            Name = "wrongful medication case",
            Description = "Child placement situation where wrongful medication is present",
            TagLabelName = "Cases",
            Text = "Children in foster care can have serious mental health issues. Too often these children are given large doses of psychotropic medications, just to keep them quiet.",
            CaseRelationTypeIds = new List<int>
            {
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.AUTHORITIES_CASE_TYPE
                }),
            }
        };
        yield return new CaseType {
            Id = Constants.WRONGFUL_REMOVAL_CASE,
            AuthorSpecific = false,
            Name = "wrongful removal case",
            Description = "Children wrongfully removed from their family",
            TagLabelName = "Cases",
            Text = "The removal of children from their family's should always be a last resort. These cases demonstrate that Child Protective Services sometimes remove children for all the wrong reasons",
            CaseRelationTypeIds = new List<int>
            {
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.AUTHORITIES_CASE_TYPE
                }),
            }
        };
        yield return new CaseType {
            Id = Constants.DISRUPTED_PLACEMENT_CASE,
            AuthorSpecific = false,
            Name = "disrupted placement case",
            Description = "A situation where the placement of a child was reverted",
            TagLabelName = "Cases",
            Text = "Although the adoptive family is called the \"forever family\" by the adoption industry, adoptions can end in disruption. These cases demonstrate that the \"forever family\" is sometimes only temporary",
            CaseRelationTypeIds = new List<int>
            {
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.PLACEMENT_CASE_TYPE
                }),
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.FACILITATION_CASE_TYPE
                }),
            }
        };

    }

    private async IAsyncEnumerable<ViewNodeTypeListAction> GetViewNodeTypeListActions(IMandatorySingleItemDatabaseReader<ActionIdReaderByPathRequest, int> reader)
    {
        await Task.CompletedTask;
        yield return new ViewNodeTypeListAction {
            BasicActionId = await reader.ReadAsync(new ActionIdReaderByPathRequest {
                Path = "/organizations"
            }),
            NodeTypeId = 23
        };
        yield return new ViewNodeTypeListAction {
            BasicActionId = await reader.ReadAsync(new ActionIdReaderByPathRequest {
                Path = "/persons"
            }),
            NodeTypeId = 24
        };
        yield return new ViewNodeTypeListAction {
            BasicActionId = await reader.ReadAsync(new ActionIdReaderByPathRequest {
                Path = "/abuse_cases"
            }),
            NodeTypeId = 26
        };
        yield return new ViewNodeTypeListAction {
            BasicActionId = await reader.ReadAsync(new ActionIdReaderByPathRequest {
                Path = "/child_trafficking_cases"
            }),
            NodeTypeId = 29
        };
        yield return new ViewNodeTypeListAction {
            BasicActionId = await reader.ReadAsync(new ActionIdReaderByPathRequest {
                Path = "/coerced_adoption_cases"
            }),
            NodeTypeId = 30
        };
        yield return new ViewNodeTypeListAction {
            BasicActionId = await reader.ReadAsync(new ActionIdReaderByPathRequest {
                Path = "/deportation_cases"
            }),
            NodeTypeId = 31

        };
        yield return new ViewNodeTypeListAction {
            BasicActionId = await reader.ReadAsync(new ActionIdReaderByPathRequest {
                Path = "/fathers_rights_violation_cases"
            }),
            NodeTypeId = 32
        };
        yield return new ViewNodeTypeListAction {
            BasicActionId = await reader.ReadAsync(new ActionIdReaderByPathRequest {
                Path = "/wrongful_medication_cases"
            }),
            NodeTypeId = 33
        };
        yield return new ViewNodeTypeListAction {
            BasicActionId = await reader.ReadAsync(new ActionIdReaderByPathRequest {
                Path = "/wrongful_removal_cases"
            }),
            NodeTypeId = 34
        };
        yield return new ViewNodeTypeListAction {
            BasicActionId = await reader.ReadAsync(new ActionIdReaderByPathRequest {
                Path = "/disrupted_placement_cases"
            }),
            NodeTypeId = 44
        };
    }


}
