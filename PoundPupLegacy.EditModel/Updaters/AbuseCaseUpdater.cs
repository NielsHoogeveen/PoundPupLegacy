namespace PoundPupLegacy.EditModel.Updaters;

using Request = AbuseCaseUpdaterRequest;

public sealed record AbuseCaseUpdaterRequest : IRequest
{
    public required int NodeId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required FuzzyDate? Date { get; init; }
    public required int ChildPlacementTypeId { get; init; }
    public required int? FamilySizeId { get; init; }
    public required bool? HomeschoolingInvolved { get; init; }
    public required bool? FundamentalFaithInvolved { get; init; }
    public required bool? DisabilitiesInvolved { get; init; }

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
            ParameterValue.Create(NodeId, request.NodeId),
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

