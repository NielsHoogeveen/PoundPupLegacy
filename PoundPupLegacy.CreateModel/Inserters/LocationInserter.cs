namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class LocationInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Location, LocationInserter>
{
    internal static AutoGenerateIntegerDatabaseParameter Id = new() { Name = "id" };
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

internal sealed class LocationInserter : ConditionalAutoGenerateIdDatabaseInserter<Location>
{
    public LocationInserter(NpgsqlCommand command, NpgsqlCommand autoGenerateIdCommand) : base(command, autoGenerateIdCommand)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Location item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(LocationInserterFactory.Id, item.Id),
            ParameterValue.Create(LocationInserterFactory.Street, item.Street),
            ParameterValue.Create(LocationInserterFactory.Additional, item.Additional),
            ParameterValue.Create(LocationInserterFactory.City, item.City),
            ParameterValue.Create(LocationInserterFactory.PostalCode, item.PostalCode),
            ParameterValue.Create(LocationInserterFactory.SubdivisionId, item.SubdivisionId),
            ParameterValue.Create(LocationInserterFactory.CountryId, item.CountryId),
            ParameterValue.Create(LocationInserterFactory.Latitude, item.Latitude),
            ParameterValue.Create(LocationInserterFactory.Longitude, item.Longitude)
        };
    }
}
