namespace PoundPupLegacy.Convert;

internal sealed class ActionMigrator(
    IDatabaseConnections databaseConnections,
    IEntityCreator<BasicAction> basicActionCreator,
    IEntityCreator<CreateNodeAction> createNodeActionCreator,
    IEntityCreator<DeleteNodeAction> deleteNodeActionCreator,
    IEntityCreator<EditNodeAction> editNodeActionCreator,
    IEntityCreator<EditOwnNodeAction> editOwnNodeActionCreator
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "actions";
    protected override async Task MigrateImpl()
    {
        await basicActionCreator.CreateAsync(GetBasicActions(), _postgresConnection);
        await createNodeActionCreator.CreateAsync(NodeTypeMigrator.GetNodeTypes().Select(x => new CreateNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await deleteNodeActionCreator.CreateAsync(NodeTypeMigrator.GetNodeTypes().Select(x => new DeleteNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await editNodeActionCreator.CreateAsync(NodeTypeMigrator.GetNodeTypes().Select(x => new EditNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await editOwnNodeActionCreator.CreateAsync(NodeTypeMigrator.GetNodeTypes().Where(x => x.AuthorSpecific).Select(x => new EditOwnNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
    }

    private async IAsyncEnumerable<BasicAction> GetBasicActions()
    {
        await Task.CompletedTask;
        yield return new BasicAction {
            Id = null,
            Path = "/organizations",
            Description = "Show all organizations"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/persons",
            Description = "Show all persons"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/abuse_cases",
            Description = "Show all abuse cases"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/child_trafficking_cases",
            Description = "Show all child trafficking cases"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/coerced_adoption_cases",
            Description = "Show all coerced adoption cases"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/deportation_cases",
            Description = "Show all deportation cases"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/fathers_rights_violation_cases",
            Description = "Show all father's rights violation cases"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/wrongful_medication_cases",
            Description = "Show all wrongful medication cases"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/wrongful_removal_cases",
            Description = "Show all wrongful removal cases"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/disrupted_placement_cases",
            Description = "Show all disrupted placement cases"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/documents",
            Description = "Show all documents"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/blogs",
            Description = "Show all blogs"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/topics",
            Description = "Show all topics"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/countries",
            Description = "Show all topics"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/cases",
            Description = "Show all cases"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/contact",
            Description = "Contact page"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/search",
            Description = "Full text search"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/polls",
            Description = "Show all polls"
        };

        yield return new BasicAction {
            Id = null,
            Path = "/node/{Id:int}",
            Description = "View a node"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/blog/{Id:int}",
            Description = "View a blog"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/united_states_congress",
            Description = "View information about the United States congress"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/united_states_senate/{Id:int}",
            Description = "View information about the United States Senate"
        };
        yield return new BasicAction {
            Id = null,
            Path = "/united_states_house_of_representatives/{Id:int}",
            Description = "View information about the United States House of Representatives"
        };
    }
}
