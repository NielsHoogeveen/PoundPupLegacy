namespace PoundPupLegacy.CreateModel.Inserters;

using Request = LocationLocatable;

internal sealed class LocationLocatableInserterFactory : BasicDatabaseInserterFactory<Request>
{
    private static readonly NullCheckingIntegerDatabaseParameter LocationId = new() { Name = "location_id" };
    private static readonly NonNullableIntegerDatabaseParameter LocatableId = new() { Name = "locatable_id" };

    public override string TableName => "location_locatable";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(LocationId, request.LocationId),
            ParameterValue.Create(LocatableId, request.LocatableId),
        };
    }
}
