namespace PoundPupLegacy.DomainModel.Updaters;

using PoundPupLegacy.DomainModel;
using Request = UnitedStatesCity.ToUpdate;

internal sealed class UnitedStatesCityChangerFactory(
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
internal sealed class UnitedStatesCityUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    private static readonly NonNullableDecimalDatabaseParameter Latitude = new() { Name = "latitude" };
    private static readonly NonNullableDecimalDatabaseParameter Longitude = new() { Name = "longitude" };
    private static readonly NonNullableIntegerDatabaseParameter Population = new() { Name = "population" };
    private static readonly NonNullableDoubleDatabaseParameter Density = new() { Name = "density" };
    private static readonly NonNullableBooleanDatabaseParameter Military = new() { Name = "military" };
    private static readonly NonNullableBooleanDatabaseParameter Incorporated = new() { Name = "incorporated" };
    private static readonly NonNullableStringDatabaseParameter Timezone = new() { Name = "timezone" };
    private static readonly NonNullableStringDatabaseParameter SimpleName = new() { Name = "simple_name" };
    private static readonly NonNullableIntegerDatabaseParameter CountyId = new() { Name = "county_id" };


    public override string Sql => $"""
        update node 
        set 
            title=@title
        where id = @node_id;
        update nameable 
        set 
            description=@description
        where id = @node_id;
        update united_states_city
        set
        latitude = @latitude,
        longitude = @longitude,
        population = @population,
        density = @density,
        military = @military,
        incorporated = @incorporated,
        timezone = @timezone,
        simple_name = @simple_name,
        county_id = @county_id
        where id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.Identification.Id),
            ParameterValue.Create(Title, request.NodeDetails.Title),
            ParameterValue.Create(Description, request.NameableDetails.Description),
            ParameterValue.Create(Latitude, request.Latitude),
            ParameterValue.Create(Longitude, request.Longitude),
            ParameterValue.Create(Population, request.Population),
            ParameterValue.Create(Density, request.Density),
            ParameterValue.Create(Military, request.Military),
            ParameterValue.Create(Incorporated, request.Incorporated),
            ParameterValue.Create(Timezone, request.Timezone),
            ParameterValue.Create(SimpleName, request.SimpleName),
            ParameterValue.Create(CountyId, request.UnitedStatesCountyId),

        };
    }
}