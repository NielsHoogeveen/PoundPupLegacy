namespace PoundPupLegacy.DomainModel.Inserters;

using Request = UnitedStatesCity.ToCreate;

internal sealed class UnitedStatesCityInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableDecimalDatabaseParameter Latitude = new() { Name = "latitude" };
    private static readonly NonNullableDecimalDatabaseParameter Longitude = new() { Name = "longitude" };
    private static readonly NonNullableIntegerDatabaseParameter Population = new() { Name = "population" };
    private static readonly NonNullableDoubleDatabaseParameter Density = new() { Name = "density" };
    private static readonly NonNullableBooleanDatabaseParameter Military = new() { Name = "military" };
    private static readonly NonNullableBooleanDatabaseParameter Incorporated = new() { Name = "incorporated" };
    private static readonly NonNullableStringDatabaseParameter Timezone = new() { Name = "timezone" };
    private static readonly NonNullableStringDatabaseParameter SimpleName = new() { Name = "simple_name" };
    private static readonly NonNullableIntegerDatabaseParameter CountyId = new() { Name = "county_id" };

    public override string TableName => "united_states_city";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
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
