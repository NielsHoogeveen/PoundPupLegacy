using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal sealed class ActionMigrator : PPLMigrator
{
    protected override string Name => "actions";

    public ActionMigrator(MySqlToPostgresConverter converter) : base(converter)
    {

    }
    protected override async Task MigrateImpl()
    {
        await BasicActionCreator.CreateAsync(GetBasicActions(), _postgresConnection);
        await CreateNodeActionCreator.CreateAsync(NodeTypeMigrator.GetNodeTypes().Select(x => new CreateNodeAction { Id = null, NodeTypeId = x.Id }), _postgresConnection);
        await DeleteNodeActionCreator.CreateAsync(NodeTypeMigrator.GetNodeTypes().Select(x => new DeleteNodeAction { Id = null, NodeTypeId = x.Id }), _postgresConnection);
        await EditNodeActionCreator.CreateAsync(NodeTypeMigrator.GetNodeTypes().Select(x => new EditNodeAction { Id = null, NodeTypeId = x.Id }), _postgresConnection);
    }

    private async IAsyncEnumerable<BasicAction> GetBasicActions()
    {
        await Task.CompletedTask;
        yield return new BasicAction
        {
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
            Path = "/cases",
            Description = "Show all cases"
        };
        yield return new BasicAction
        {
            Id = null,
            Path = "/abuse_cases",
            Description = "Show all abuse cases"
        };
        yield return new BasicAction
        {
            Id = null,
            Path = "/child_trafficking_cases",
            Description = "Show all child trafficking cases"
        };
        yield return new BasicAction
        {
            Id = null,
            Path = "/deportation_cases",
            Description = "Show all deportation cases"
        };
        yield return new BasicAction
        {
            Id = null,
            Path = "/wrongful_removal_cases",
            Description = "Show all wrongful removal cases"
        };
        yield return new BasicAction
        {
            Id = null,
            Path = "/wrongful_medication_cases",
            Description = "Show all wrongful medication cases"
        };
        yield return new BasicAction
        {
            Id = null,
            Path = "/fathers_rights_violation_cases",
            Description = "Show all father's rights violation cases"
        };
        yield return new BasicAction
        {
            Id = null,
            Path = "/coerced_adoption_cases",
            Description = "Show all coerced adoption cases"
        };
        yield return new BasicAction
        {
            Id = null,
            Path = "/contact",
            Description = "Contact page"
        };
        yield return new BasicAction
        {
            Id = null,
            Path = "/search",
            Description = "Full text search"
        };
    }
}
