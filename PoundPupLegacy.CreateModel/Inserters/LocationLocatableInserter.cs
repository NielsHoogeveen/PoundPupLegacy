namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class LocationLocatableInserterFactory : DatabaseInserterFactory<LocationLocatable, LocationLocatableInserter>
{
    internal static NonNullableIntegerDatabaseParameter LocationId = new() { Name = "location_id" };
    internal static NonNullableIntegerDatabaseParameter LocatableId = new() { Name = "locatable_id" };

    public override string TableName => "location_locatable";

}
internal sealed class LocationLocatableInserter : DatabaseInserter<LocationLocatable>
{
    public LocationLocatableInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(LocationLocatable item)
    {
        if(item.LocationId == null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(LocationLocatableInserterFactory.LocationId, item.LocationId.Value),
            ParameterValue.Create(LocationLocatableInserterFactory.LocatableId, item.LocatableId),
        };
    }
}
