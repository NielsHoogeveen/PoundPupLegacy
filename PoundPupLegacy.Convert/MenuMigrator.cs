namespace PoundPupLegacy.Convert;

internal sealed class MenuMigrator : MigratorPPL
{
    protected override string Name => "menu items";

    private readonly IMandatorySingleItemDatabaseReaderFactory<ActionIdReaderByPathRequest, int> _actionIdReaderByPathFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<CreateNodeActionIdReaderByNodeTypeIdRequest, int> _createNodeActionIdReaderByNodeTypeIdFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<TenantNodeIdReaderByUrlIdRequest, int> _tenantNodeIdReaderByUrlIdFactory;
    private readonly IEntityCreator<TenantNodeMenuItem> _tenantNodeMenuItemCreator;
    private readonly IEntityCreator<ActionMenuItem> _actionMenuItemCreator;

    public MenuMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<ActionIdReaderByPathRequest, int> actionIdReaderByPathFactory,
        IMandatorySingleItemDatabaseReaderFactory<CreateNodeActionIdReaderByNodeTypeIdRequest, int> createNodeActionIdReaderByNodeTypeIdFactory,
        IMandatorySingleItemDatabaseReaderFactory<TenantNodeIdReaderByUrlIdRequest, int> tenantNodeIdReaderByUrlIdFactory,
        IEntityCreator<TenantNodeMenuItem> tenantNodeMenuItemCreator,
        IEntityCreator<ActionMenuItem> actionMenuItemCreator

    ) : base(databaseConnections)
    {
        _actionIdReaderByPathFactory = actionIdReaderByPathFactory;
        _createNodeActionIdReaderByNodeTypeIdFactory = createNodeActionIdReaderByNodeTypeIdFactory;
        _tenantNodeIdReaderByUrlIdFactory = tenantNodeIdReaderByUrlIdFactory;
        _tenantNodeMenuItemCreator = tenantNodeMenuItemCreator;
        _actionMenuItemCreator = actionMenuItemCreator;
    }
    protected override async Task MigrateImpl()
    {
        await using var actionIdReaderByPath = await _actionIdReaderByPathFactory.CreateAsync(_postgresConnection);
        await using var createNodeActionIdReaderByNodeTypeId = await _createNodeActionIdReaderByNodeTypeIdFactory.CreateAsync(_postgresConnection);

        await _tenantNodeMenuItemCreator.CreateAsync(GetTenantNodeMenuItems(), _postgresConnection);
        await _actionMenuItemCreator.CreateAsync(GetActionMenuItems(actionIdReaderByPath, createNodeActionIdReaderByNodeTypeId), _postgresConnection);
    }

    private async IAsyncEnumerable<ActionMenuItem> GetActionMenuItems(
        IMandatorySingleItemDatabaseReader<ActionIdReaderByPathRequest, int> actionIdReaderByPath,
        IMandatorySingleItemDatabaseReader<CreateNodeActionIdReaderByNodeTypeIdRequest, int> createNodeActionIdReaderByNodeTypeId
        )
    {
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/contact" }),
            Name = "Contact",
            Weight = 2,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/documents" }),
            Name = "Documents",
            Weight = 3,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/blogs" }),
            Name = "Blogs",
            Weight = 4,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/cases" }),
            Name = "Cases",
            Weight = 5,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/topics" }),
            Name = "Topics",
            Weight = 6,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/countries" }),
            Name = "Countries",
            Weight = 7,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/organizations" }),
            Name = "Organizations",
            Weight = 8,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/persons" }),
            Name = "Persons",
            Weight = 9,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync(new ActionIdReaderByPathRequest { Path = "/polls" }),
            Name = "Polls",
            Weight = 10,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest 
            { 
                NodeTypeId = Constants.BLOG_POST 
            }),
            Name = "Create Blog",
            Weight = 11,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest 
            { 
                NodeTypeId = Constants.DOCUMENT 
            }),
            Name = "Create Document",
            Weight = 12,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest 
            { 
                NodeTypeId = Constants.DISCUSSION 
            }),
            Name = "Create Discussion",
            Weight = 13,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest 
            { 
                NodeTypeId = Constants.ORGANIZATION 
            }),
            Name = "Create Organization",
            Weight = 14,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest 
            { 
                NodeTypeId = Constants.PERSON 
            }),
            Name = "Create Person",
            Weight = 15,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest 
            { 
                NodeTypeId = Constants.ABUSE_CASE 
            }),
            Name = "Create Abuse Case",
            Weight = 16,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest 
            { 
                NodeTypeId = Constants.CHILD_TRAFFICKING_CASE 
            }),
            Name = "Create Child Trafficking Case",
            Weight = 17,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest 
            { 
                NodeTypeId = Constants.COERCED_ADOPTION_CASE 
            }),
            Name = "Create Coerced Adoption Case",
            Weight = 18,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.DEPORTATION_CASE
            }),
            Name = "Create Deportation Case",
            Weight = 19,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.DISRUPTED_PLACEMENT_CASE
            }),
            Name = "Create Disrupted Placement Case",
            Weight = 20,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.FATHERS_RIGHTS_VIOLATION_CASE
            }),
            Name = "Create Father's rights violation Case",
            Weight = 21,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.WRONGFUL_MEDICATION_CASE
            }),
            Name = "Create Wrongful Medication Case",
            Weight = 22,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(new CreateNodeActionIdReaderByNodeTypeIdRequest {
                NodeTypeId = Constants.WRONGFUL_REMOVAL_CASE
            }),
            Name = "Create Wrongful Removal Case",
            Weight = 23,
        };

    }
    private async IAsyncEnumerable<TenantNodeMenuItem> GetTenantNodeMenuItems()
    {
        await using var tenantNodeIdByUrlIdReader = await _tenantNodeIdReaderByUrlIdFactory.CreateAsync(_postgresConnection);
        yield return new TenantNodeMenuItem {
            Id = null,
            TenantNodeId = await tenantNodeIdByUrlIdReader.ReadAsync(new TenantNodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = 1063,
            }),
            Name = "About Us",
            Weight = 0,
        };
        yield return new TenantNodeMenuItem {
            Id = null,
            TenantNodeId = await tenantNodeIdByUrlIdReader.ReadAsync(new TenantNodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = 34428,
            }),
            Name = "Our Position",
            Weight = 1,
        };
    }

}
