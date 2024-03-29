﻿using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Updaters;

using PoundPupLegacy.DomainModel.Creators;
using Request = FathersRightsViolationCase.ToUpdate;
internal sealed class FathersRightsViolationCaseChangerFactory(
    IDatabaseUpdaterFactory<Request> databaseUpdaterFactory,
    CaseDetailsChangerFactory caseDetailsChangerFactory,
    NodeDetailsChangerFactory nodeDetailsChangerFactory,
    IDatabaseUpdaterFactory<LocationUpdaterRequest> locationUpdaterFactory,
    IDatabaseInserterFactory<Location.ToCreate> locationInserterFactory,
    IDatabaseInserterFactory<LocationLocatable> locationLocatableInserterFactory,
    IDatabaseUpdaterFactory<Term.ToUpdate> termUpdaterFactory,
    DatabaseMaterializedViewRefresherFactory termViewRefresherFactory
) : IEntityChangerFactory<Request>
{
    public async Task<IEntityChanger<Request>> CreateAsync(IDbConnection connection)
    {
        return new CaseChanger<Request>(
            await databaseUpdaterFactory.CreateAsync(connection),
            await caseDetailsChangerFactory.CreateAsync(connection),
            await nodeDetailsChangerFactory.CreateAsync(connection),
            await locationUpdaterFactory.CreateAsync(connection),
            await locationInserterFactory.CreateAsync(connection),
            await locationLocatableInserterFactory.CreateAsync(connection),
            await termUpdaterFactory.CreateAsync(connection),
            await termViewRefresherFactory.CreateAsync(connection, "nameable_descendency")
        );
    }
}

internal sealed class FathersRightsViolationCaseUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    private static readonly NullableFuzzyDateDatabaseParameter FuzzyDate = new() { Name = "fuzzy_date" };

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
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.Identification.Id),
            ParameterValue.Create(Title, request.NodeDetails.Title),
            ParameterValue.Create(Description, request.NameableDetails.Description),
            ParameterValue.Create(FuzzyDate, request.CaseDetails.Date),
        };
    }
}

