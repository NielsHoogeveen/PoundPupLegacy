namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = LocationLocatableInserterFactory;
using Request = LocationLocatable;
using Inserter = LocationLocatableInserter;


internal sealed class LocationLocatableInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NullCheckingIntegerDatabaseParameter LocationId = new() { Name = "location_id" };
    internal static NonNullableIntegerDatabaseParameter LocatableId = new() { Name = "locatable_id" };

    public override string TableName => "location_locatable";

}
internal sealed class LocationLocatableInserter : DatabaseInserter<Request>
{
    public LocationLocatableInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.LocationId, request.LocationId),
            ParameterValue.Create(Factory.LocatableId, request.LocatableId),
        };
    }
}
