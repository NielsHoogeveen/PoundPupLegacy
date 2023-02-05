using PoundPupLegacy.Model;
using PoundPupLegacy.Db;

namespace PoundPupLegacy.Convert;

internal sealed class ActionMigrator : PPLMigrator
{
    protected override string Name => "actions";

    public ActionMigrator(MySqlToPostgresConverter converter): base(converter)
    {

    }
    protected override async Task MigrateImpl()
    {
        await BasicActionCreator.CreateAsync(GetBasicActions(), _postgresConnection);
        await CreateNodeActionCreator.CreateAsync(NodeTypeMigrator.GetNodeTypes().Select(x => new CreateNodeAction { Id = null, NodeTypeId = x.Id }), _postgresConnection);
        await DeleteNodeActionCreator.CreateAsync(NodeTypeMigrator.GetNodeTypes().Select(x => new DeleteNodeAction { Id = null, NodeTypeId = x.Id }), _postgresConnection);
        await EditNodeActionCreator.CreateAsync(NodeTypeMigrator.GetNodeTypes().Select(x => new EditNodeAction { Id = null, NodeTypeId = x.Id }), _postgresConnection);
    }

    private async IAsyncEnumerable<BasicAction> GetBasicActions() {
        await Task.CompletedTask;
        yield return new BasicAction {
            Id = null,
            Path = "/articles",
            Description = "Show all articles"
        };
        yield return new BasicAction
        {
            Id = null,
            Path = "/blogs",
            Description = "Show all blogs"
        };
        yield return new BasicAction
        {
            Id = null,
            Path = "/organizations",
            Description = "Show all organizations"
        };
        yield return new BasicAction
        {
            Id = null,
            Path = "/persons",
            Description = "Show all persons"
        };
        yield return new BasicAction
        {
            Id = null,
            Path = "/topics",
            Description = "Show all topics"
        };
        yield return new BasicAction
        {
            Id = null,
            Path = "/countries",
            Description = "Show all topics"
        };
        yield return new BasicAction
        {
            Id = null,
            Path = "/contact",
            Description = "Contact page"
        };
    }
}
