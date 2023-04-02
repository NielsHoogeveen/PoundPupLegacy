namespace PoundPupLegacy.Convert;

internal sealed class MenuMigrator : MigratorPPL
{
    protected override string Name => "menu items";

    private readonly IDatabaseReaderFactory<ActionIdReaderByPath> _actionIdReaderByPathFactory;
    private readonly IDatabaseReaderFactory<CreateNodeActionIdReaderByNodeTypeId> _createNodeActionIdReaderByNodeTypeIdFactory;
    private readonly IDatabaseReaderFactory<TenantNodeIdReaderByUrlId> _tenantNodeIdReaderByUrlIdFactory;
    private readonly IEntityCreator<TenantNodeMenuItem> _tenantNodeMenuItemCreator;
    private readonly IEntityCreator<ActionMenuItem> _actionMenuItemCreator;

    public MenuMigrator(
        IDatabaseConnections databaseConnections,
        IDatabaseReaderFactory<ActionIdReaderByPath> actionIdReaderByPathFactory,
        IDatabaseReaderFactory<CreateNodeActionIdReaderByNodeTypeId> createNodeActionIdReaderByNodeTypeIdFactory,
        IDatabaseReaderFactory<TenantNodeIdReaderByUrlId> tenantNodeIdReaderByUrlIdFactory,
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

    private async IAsyncEnumerable<ActionMenuItem> GetActionMenuItems(ActionIdReaderByPath actionIdReaderByPath, CreateNodeActionIdReaderByNodeTypeId createNodeActionIdReaderByNodeTypeId)
    {
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync("/contact"),
            Name = "Contact",
            Weight = 2,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync("/articles"),
            Name = "Articles",
            Weight = 3,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync("/blogs"),
            Name = "Blogs",
            Weight = 4,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync("/cases"),
            Name = "Cases",
            Weight = 5,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync("/topics"),
            Name = "Topics",
            Weight = 6,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync("/countries"),
            Name = "Countries",
            Weight = 7,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync("/organizations"),
            Name = "Organizations",
            Weight = 8,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync("/persons"),
            Name = "Persons",
            Weight = 9,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await actionIdReaderByPath.ReadAsync("/polls"),
            Name = "Polls",
            Weight = 10,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(35),
            Name = "Create Blog",
            Weight = 11,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(36),
            Name = "Create Article",
            Weight = 12,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(37),
            Name = "Create Discussion",
            Weight = 13,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(26),
            Name = "Create Abuse Case",
            Weight = 14,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(29),
            Name = "Create Child Trafficking Case",
            Weight = 15,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(23),
            Name = "Create Organization",
            Weight = 16,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await createNodeActionIdReaderByNodeTypeId.ReadAsync(24),
            Name = "Create Person",
            Weight = 17,
        };
    }
    private async IAsyncEnumerable<TenantNodeMenuItem> GetTenantNodeMenuItems()
    {
        await using var tenantNodeIdByUrlIdReader = await _tenantNodeIdReaderByUrlIdFactory.CreateAsync(_postgresConnection);
        yield return new TenantNodeMenuItem {
            Id = null,
            TenantNodeId = await tenantNodeIdByUrlIdReader.ReadAsync(new TenantNodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = 1063,
            }),
            Name = "About Us",
            Weight = 0,
        };
        yield return new TenantNodeMenuItem {
            Id = null,
            TenantNodeId = await tenantNodeIdByUrlIdReader.ReadAsync(new TenantNodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = 34428,
            }),
            Name = "Our Position",
            Weight = 1,
        };
    }

}
