namespace PoundPupLegacy.DomainModel.Updaters;

using PoundPupLegacy.DomainModel;
using Request = ChildPlacementType.ToUpdate;

internal sealed class ChildPlacementTypeChangerFactory(
    IDatabaseUpdaterFactory<Request> databaseUpdaterFactory,
    NodeDetailsChangerFactory nodeDetailsChangerFactory,
    IDatabaseUpdaterFactory<Term.ToUpdate> termUpdaterFactory,
    DatabaseMaterializedViewRefresherFactory termViewRefresherFactory
) : IEntityChangerFactory<Request>
{
    public async Task<IEntityChanger<Request>> CreateAsync(IDbConnection connection)
    {
        return new NameableChanger<Request>(
            await databaseUpdaterFactory.CreateAsync(connection),
            await nodeDetailsChangerFactory.CreateAsync(connection),
            await termUpdaterFactory.CreateAsync(connection),
            await termViewRefresherFactory.CreateAsync(connection, "nameable_descendency")
        );
    }
}
internal sealed class ChildPlacementTypeUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NonNullableStringDatabaseParameter Description = new() { Name = "description" };

    public override string Sql => $"""
        update node 
        set 
            title=@title
        where id = @node_id;
        update nameable 
        set 
            description=@description
        where id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.Identification.Id),
            ParameterValue.Create(Title, request.NodeDetails.Title),
            ParameterValue.Create(Description, request.NameableDetails.Description),
        };
    }
}