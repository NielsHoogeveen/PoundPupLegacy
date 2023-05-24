namespace PoundPupLegacy.CreateModel.Updaters;

using PoundPupLegacy.Common;
using Request = ImmediatelyIdentifiableAbuseCase;


internal sealed class AbuseCaseUpdater(
    IDatabaseUpdaterFactory<Request> abuseCaseUpdaterFactory,
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableTenantNode> tenantNodeUpdater,
    IDatabaseDeleterFactory<ImmediatelyIdentifiableTenantNode> tenantNodeDeleter,
    IDatabaseInserterFactory<NewTenantNodeForExistingNode> tenantNodeInserter,
    IDatabaseInserterFactory<NodeTerm> nodeTermInserter,
    IDatabaseDeleterFactory<NodeTerm> nodeTermDeleter
    ) : EntityUpdater<Request>(tenantNodeUpdater, tenantNodeDeleter, tenantNodeInserter, nodeTermInserter, nodeTermDeleter)
{
    
    protected override async Task UpdateEntityAsync(Request request, IDbConnection connection)
    {
        var updater = await abuseCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(request);
    }
}
internal sealed class AbuseCaseUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    private static readonly NullableFuzzyDateDatabaseParameter FuzzyDate = new() { Name = "fuzzy_date" };
    private static readonly NonNullableIntegerDatabaseParameter ChildPlacementTypeId = new() { Name = "child_placement_type_id" };
    private static readonly NullableIntegerDatabaseParameter FamilySizeId = new() { Name = "family_size_id" };
    private static readonly NullableBooleanDatabaseParameter HomeSchoolingInvolved = new() { Name = "home_schooling_involved" };
    private static readonly NullableBooleanDatabaseParameter FundamentalFaithInvolved = new() { Name = "fundamental_faith_involved" };
    private static readonly NullableBooleanDatabaseParameter DisabilitiesInvolved = new() { Name = "disabilities_involved" };

    public override string Sql => $"""
        update node 
        set 
            title=@title
        where id = @node_id;
        update nameable 
        set 
            description=@description
        where id = @node_id;
        update "case"
        set 
            fuzzy_date=@fuzzy_date
        where id = @node_id;
        update abuse_case 
        set 
            child_placement_type_id=@child_placement_type_id,
            family_size_id= @family_size_id,
            home_schooling_involved=@home_schooling_involved,
            fundamental_faith_involved= @fundamental_faith_involved,
            disabilities_involved=@disabilities_involved
        where id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.Id),
            ParameterValue.Create(Title, request.Title),
            ParameterValue.Create(Description, request.Description),
            ParameterValue.Create(FuzzyDate, request.Date),
            ParameterValue.Create(ChildPlacementTypeId, request.ChildPlacementTypeId),
            ParameterValue.Create(FamilySizeId, request.FamilySizeId),
            ParameterValue.Create(HomeSchoolingInvolved, request.HomeschoolingInvolved),
            ParameterValue.Create(FundamentalFaithInvolved, request.FundamentalFaithInvolved),
            ParameterValue.Create(DisabilitiesInvolved, request.DisabilitiesInvolved),
        };
    }
}

