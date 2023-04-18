namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Nameable;

internal sealed class NameableInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NullableIntegerDatabaseParameter FileIdTileImage = new() { Name = "file_id_tile_image" };

    public override string TableName => "nameable";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Description, request.Description),
            ParameterValue.Create(FileIdTileImage, request.FileIdTileImage),
        };
    }
}
