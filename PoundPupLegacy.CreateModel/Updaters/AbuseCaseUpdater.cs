﻿namespace PoundPupLegacy.CreateModel.Updaters;

using PoundPupLegacy.Common;
using System.Data;
using System.Threading.Tasks;
using Request = ImmediatelyIdentifiableAbuseCase;

internal sealed class AbuseCaseChangerFactory(
    IDatabaseUpdaterFactory<Request> databaseUpdaterFactory,
    NodeDetailsChangerFactory nodeDetailsChangerFactory) : IEntityChangerFactory<Request>
{
    public async Task<IEntityChanger<Request>> CreateAsync(IDbConnection connection)
    {
        return new AbuseCaseChanger(
            await databaseUpdaterFactory.CreateAsync(connection),
            await nodeDetailsChangerFactory.CreateAsync(connection)
        );
    }
}
internal sealed class AbuseCaseChanger(
    IDatabaseUpdater<Request> databaseUpdater,
    NodeDetailsChanger nodeDetailsChanger
    ) : NodeChanger<Request>(databaseUpdater, nodeDetailsChanger)
{
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
