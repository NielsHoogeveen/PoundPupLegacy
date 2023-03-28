using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal sealed class MenuMigrator : PPLMigrator
{
    protected override string Name => "menu items";

    public MenuMigrator(MySqlToPostgresConverter converter) : base(converter)
    {

    }
    protected override async Task MigrateImpl()
    {
        await TenantNodeMenuItemCreator.CreateAsync(GetTenantNodeMenuItems(), _postgresConnection);
        await ActionMenuItemCreator.CreateAsync(GetActionMenuItems(), _postgresConnection);
    }

    private async IAsyncEnumerable<ActionMenuItem> GetActionMenuItems()
    {
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _actionReaderByPath.ReadAsync("/contact"),
            Name = "Contact",
            Weight = 2,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _actionReaderByPath.ReadAsync("/articles"),
            Name = "Articles",
            Weight = 3,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _actionReaderByPath.ReadAsync("/blogs"),
            Name = "Blogs",
            Weight = 4,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _actionReaderByPath.ReadAsync("/cases"),
            Name = "Cases",
            Weight = 5,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _actionReaderByPath.ReadAsync("/topics"),
            Name = "Topics",
            Weight = 6,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _actionReaderByPath.ReadAsync("/countries"),
            Name = "Countries",
            Weight = 7,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _actionReaderByPath.ReadAsync("/organizations"),
            Name = "Organizations",
            Weight = 8,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _actionReaderByPath.ReadAsync("/persons"),
            Name = "Persons",
            Weight = 9,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _actionReaderByPath.ReadAsync("/polls"),
            Name = "Polls",
            Weight = 10,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(35),
            Name = "Create Blog",
            Weight = 11,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(36),
            Name = "Create Article",
            Weight = 12,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(37),
            Name = "Create Discussion",
            Weight = 13,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(26),
            Name = "Create Abuse Case",
            Weight = 14,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(29),
            Name = "Create Child Trafficking Case",
            Weight = 15,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(23),
            Name = "Create Organization",
            Weight = 16,
        };
        yield return new ActionMenuItem {
            Id = null,
            ActionId = await _createNodeActionIdReaderByNodeTypeId.ReadAsync(24),
            Name = "Create Person",
            Weight = 17,
        };
    }
    private async IAsyncEnumerable<TenantNodeMenuItem> GetTenantNodeMenuItems()
    {
        await Task.CompletedTask;
        yield return new TenantNodeMenuItem {
            Id = null,
            TenantNodeId = await _tenantNodeIdByUrlIdReader.ReadAsync(new Db.Readers.TenantNodeIdReaderByUrlId.TenantNodeIdReaderByUrlIdRequest 
            { 
                TenantId = Constants.PPL,
                UrlId = 1063,
            }),
            Name = "About Us",
            Weight = 0,
        };
        yield return new TenantNodeMenuItem {
            Id = null,
            TenantNodeId = await _tenantNodeIdByUrlIdReader.ReadAsync(new Db.Readers.TenantNodeIdReaderByUrlId.TenantNodeIdReaderByUrlIdRequest 
            { 
                TenantId = Constants.PPL,
                UrlId = 34428,
            }),
            Name = "Our Position",
            Weight = 1,
        };
    }

}
