namespace PoundPupLegacy.EditModel.Deleters;

using Request = LocationDeleterRequest;

public sealed record LocationDeleterRequest : IRequest
{
    public required int LocationId { get; init; }
    public required int LocatableId { get; init; }
}

internal sealed class LocationDeleterFactory : DatabaseDeleterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter LocationId = new() { Name = "location_id" };
    private static readonly NonNullableIntegerDatabaseParameter LocatableId = new() { Name = "locatable_id" };
    public override string Sql => SQL;

    const string SQL = $"""
        delete from location_locatable
        where location_id = @location_id and locatable_id = @locatable_id;
        delete from location
        where id in (
            select
            l.id
            from location l 
            left join location_locatable ll on l.id = ll.location_id
            where ll.location_id is null
        )
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(LocationId, request.LocationId),
            ParameterValue.Create(LocatableId, request.LocatableId),
        };
    }
}
