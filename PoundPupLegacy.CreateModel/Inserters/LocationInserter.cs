namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = LocationInserterFactory;
using Request = Location;
using Inserter = LocationInserter;

internal sealed class LocationInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request, Inserter>
{
    internal static NullableStringDatabaseParameter Street = new() { Name = "street" };
    internal static NullableStringDatabaseParameter Additional = new() { Name = "additional" };
    internal static NullableStringDatabaseParameter City = new() { Name = "city" };
    internal static NullableStringDatabaseParameter PostalCode = new() { Name = "postal_code" };
    internal static NullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    internal static NullableDecimalDatabaseParameter Latitude = new() { Name = "latitude" };
    internal static NullableDecimalDatabaseParameter Longitude = new() { Name = "longitude" };

    public override string TableName => "location";
}

internal sealed class LocationInserter : ConditionalAutoGenerateIdDatabaseInserter<Request>
{
    public LocationInserter(NpgsqlCommand command, NpgsqlCommand autoGenerateIdCommand) : base(command, autoGenerateIdCommand)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Street, request.Street),
            ParameterValue.Create(Factory.Additional, request.Additional),
            ParameterValue.Create(Factory.City, request.City),
            ParameterValue.Create(Factory.PostalCode, request.PostalCode),
            ParameterValue.Create(Factory.SubdivisionId, request.SubdivisionId),
            ParameterValue.Create(Factory.CountryId, request.CountryId),
            ParameterValue.Create(Factory.Latitude, request.Latitude),
            ParameterValue.Create(Factory.Longitude, request.Longitude)
        };
    }
}
